using UnityEditor;
using UnityEngine;
using BVA.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GLTF.Schema.BVA;
namespace BVA
{
    public class MaterialExtraGenerator : EditorWindow
    {
        [MenuItem("BVA/Developer Tools/Generator Material Extra")]
        static void Init()
        {
            MaterialExtraGenerator window = (MaterialExtraGenerator)EditorWindow.GetWindow(typeof(MaterialExtraGenerator), false, "Export Scene Settings");
            window.Show();
        }
        static string helpInfo;
        private class ExportExtraMaterialProperty
        {
            public MaterialPropertyType propertyType;
            public string propertyName;
            public string description;
            public string constParameter;
            public string parameterName;
            public bool isNormal;
        }
        Shader shaderGraphObject;
        const string DEFAULT_SCRIPT_PATH = "Assets/BVA/Runtime/Scripts/BVA/Material/";
        string exportPath = DEFAULT_SCRIPT_PATH;
        Vector3 scrollPos = Vector3.zero;
        readonly string[] excludedParameters = new string[] { "unity_Lightmaps", "unity_LightmapsInd", "unity_ShadowMasks", "_QueueOffset", "_QueueControl" };

        private static bool CheckNormalMap(string paramName)
        {
            string lowerCase = paramName.ToLower();
            return lowerCase.EndsWith("normalmap") || lowerCase.EndsWith("bumpmap");
        }
        
        void OnGUI()
        {
            shaderGraphObject = EditorGUILayout.ObjectField("Shader Graph ", shaderGraphObject, typeof(Shader), false) as Shader;
            if (shaderGraphObject == null) return;
            if (helpInfo == null)
            {
                helpInfo = $"Only these parameters can be export as NormalMap. Make sure use these parameter in order to export correctly! [{string.Join(",", GLTFSceneExporter.MATERIAL_PROPERTY_NormalMap)}] ";
            }
            EditorGUILayout.HelpBox(helpInfo, MessageType.Info);
            EditorGUILayout.TextField($"Shader : {shaderGraphObject.name}");
            int count = ShaderUtil.GetPropertyCount(shaderGraphObject);
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(Mathf.Max(0, position.width - 50)), GUILayout.Height(Mathf.Max(0, position.height - 100)));
            for (int j = 0; j < count; ++j)
            {
                var propType = ShaderUtil.GetPropertyType(shaderGraphObject, j);
                var name = ShaderUtil.GetPropertyName(shaderGraphObject, j);
                var desc = ShaderUtil.GetPropertyDescription(shaderGraphObject, j);
                if (excludedParameters.Contains(name)) //don't handle lightmap parameters
                    continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Desc : {desc}");
                EditorGUILayout.LabelField($"Name : {name}");
                EditorGUILayout.EnumPopup(propType);
                EditorGUILayout.Toggle("is normal", CheckNormalMap(name));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            exportPath = EditorGUILayout.TextField("Path : ", exportPath);
            if (GUILayout.Button("Choose", GUILayout.MaxWidth(96)))
            {
                exportPath = EditorUtility.SaveFilePanelInProject("Choose Script Path", "BVA_Material_Extra", "cs", "save", DEFAULT_SCRIPT_PATH);
            }
            if (GUILayout.Button("Gen Script", GUILayout.MaxWidth(96)))
            {
                List<ExportExtraMaterialProperty> exportProperties = new List<ExportExtraMaterialProperty>();
                for (int j = 0; j < count; ++j)
                {
                    var propType = ShaderUtil.GetPropertyType(shaderGraphObject, j);
                    var name = ShaderUtil.GetPropertyName(shaderGraphObject, j);
                    var desc = ShaderUtil.GetPropertyDescription(shaderGraphObject, j);
                    if (excludedParameters.Contains(name)) //don't handle lightmap parameters
                        continue;
                    switch (propType)
                    {
                        case ShaderUtil.ShaderPropertyType.Color:
                            exportProperties.Add(new ExportExtraMaterialProperty() { propertyName = name, description = desc, propertyType = MaterialPropertyType.Color });
                            break;
                        case ShaderUtil.ShaderPropertyType.Vector:
                            exportProperties.Add(new ExportExtraMaterialProperty() { propertyName = name, description = desc, propertyType = MaterialPropertyType.Vector });
                            break;
                        case ShaderUtil.ShaderPropertyType.Float:
                        case ShaderUtil.ShaderPropertyType.Range:
                            exportProperties.Add(new ExportExtraMaterialProperty() { propertyName = name, description = desc, propertyType = MaterialPropertyType.Float });
                            break;
                        case ShaderUtil.ShaderPropertyType.TexEnv:
                            var dimension = shaderGraphObject.GetPropertyTextureDimension(j);
                            if (dimension == UnityEngine.Rendering.TextureDimension.Tex2D)
                                exportProperties.Add(new ExportExtraMaterialProperty() { propertyName = name, description = desc, propertyType = MaterialPropertyType.Texture2D, isNormal = CheckNormalMap(name) });
                            if (dimension == UnityEngine.Rendering.TextureDimension.Cube)
                                exportProperties.Add(new ExportExtraMaterialProperty() { propertyName = name, description = desc, propertyType = MaterialPropertyType.Cubemap });
                            break;
#if UNITY_2021_1_OR_NEWER
                        case ShaderUtil.ShaderPropertyType.Int:
                            exportProperties.Add(new ExportExtraMaterialProperty() { propertyName = name, description = desc, propertyType = MaterialPropertyType.Int });
                            break;
#endif
                    }
                }
                ExportScript(shaderGraphObject.name, exportProperties);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ExportScript(string shaderName, List<ExportExtraMaterialProperty> properties)
        {
            //write using namespace
            string[] namespaceField = new string[] { "Newtonsoft.Json.Linq", "Newtonsoft.Json" , "GLTF.Extensions", "BVA.Extensions", "System.Threading.Tasks", "UnityEngine",
            "Color = UnityEngine.Color","Vector4 = UnityEngine.Vector4" };
            const string namespaceHeader = "namespace GLTF.Schema.BVA";
            const string startScope = "{", endScope = "}";
            string className = Path.GetFileNameWithoutExtension(exportPath);
            if (string.IsNullOrWhiteSpace(className))
            {
                EditorUtility.DisplayDialog("error", "need file name,and should start with BVA!", "OK");
            }
            string classHeader = $"public class {className} : MaterialExtra";
            string ParameterName(string desc) { return "parameter_" + desc.GetNumberAlphaUnderLine(); }
            StringWriter sw = new StringWriter();
            foreach (var f in namespaceField)
                sw.WriteLine($"using {f};");
            sw.WriteLine();

            sw.WriteLine(namespaceHeader);
            // namespace scope
            sw.WriteLine(startScope);
            {
                sw.WriteLine(classHeader);
                sw.WriteLine(startScope);
                {
                    // in class scope
                    // write const string for json PROPERTY and SHADER_NAME
                    sw.WriteLine($"public const string PROPERTY = \"{className}\";");
                    sw.WriteLine($"public const string SHADER_NAME = \"{shaderName}\";");
                    // write const string for Property Block
                    foreach (var v in properties)
                    {
                        v.constParameter = v.propertyName.Replace("_", "").ToUpper().Replace("1ST", "FIRST").Replace("2ND", "SECOND");
                        v.parameterName = ParameterName(v.propertyName);
                        sw.WriteLine($"public const string {v.constParameter} = \"{v.propertyName}\";");
                    }
                    // write parameter declaration
                    foreach (var v in properties)
                    {
                        if (v.propertyType == MaterialPropertyType.Texture2D)
                        {
                            sw.WriteLine($"public MaterialTextureParam {v.parameterName} = new MaterialTextureParam({v.constParameter});");
                        }
                        else if (v.propertyType == MaterialPropertyType.Color)
                        {
                            sw.WriteLine($"public MaterialParam<Color> {v.parameterName} = new MaterialParam<Color>({v.constParameter}, Color.white);");
                        }
                        else if (v.propertyType == MaterialPropertyType.Vector)
                        {
                            sw.WriteLine($"public MaterialParam<Vector4> {v.parameterName} = new MaterialParam<Vector4>({v.constParameter}, Vector4.one);");
                        }
                        else if (v.propertyType == MaterialPropertyType.Float)
                        {
                            sw.WriteLine($"public MaterialParam<float> {v.parameterName} = new MaterialParam<float>({v.constParameter}, 1.0f);");
                        }
                        if (v.propertyType == MaterialPropertyType.Cubemap)
                        {
                            sw.WriteLine($"public MaterialCubemapParam {v.parameterName} = new MaterialCubemapParam({v.constParameter});");
                        }
                        else if (v.propertyType == MaterialPropertyType.Int)
                        {
                            sw.WriteLine($"public MaterialParam<int> {v.parameterName} = new MaterialParam<int>({v.constParameter}, 1);");
                        }
                    }
                    sw.WriteLine($"public string[] keywords;");
                    // write construction function
                    sw.WriteLine($"public {className}(Material material, {nameof(ExportTextureInfo)} exportTextureInfo, {nameof(ExportTextureInfo)} exportNormalTextureInfo, {nameof(ExportCubemapInfo)} exportCubemapInfo)");

                    sw.WriteLine(startScope);

                    sw.WriteLine("keywords = material.shaderKeywords;");
                    foreach (var v in properties)
                    {
                        if (v.propertyType == MaterialPropertyType.Texture2D)
                        {
                            string tempVariable = v.parameterName.ToLower() + "_temp";
                            string exportFunc = v.isNormal ? "exportNormalTextureInfo" : "exportTextureInfo";
                            sw.WriteLine($"var {tempVariable} = material.GetTexture({v.parameterName}.ParamName);");
                            sw.WriteLine($"if ({tempVariable} != null) {v.parameterName}.Value = {exportFunc}({tempVariable});");
                        }
                        if (v.propertyType == MaterialPropertyType.Cubemap)
                        {
                            string tempVariable = v.parameterName.ToLower() + "_temp";
                            sw.WriteLine($"var {tempVariable} = material.GetTexture({v.parameterName}.ParamName);");
                            sw.WriteLine($"if ({tempVariable} != null) {v.parameterName}.Value = exportCubemapInfo({tempVariable} as Cubemap);");
                        }
                        else if (v.propertyType == MaterialPropertyType.Color)
                        {
                            sw.WriteLine($"{v.parameterName}.Value = material.GetColor({v.parameterName}.ParamName);");
                        }
                        else if (v.propertyType == MaterialPropertyType.Vector)
                        {
                            sw.WriteLine($"{v.parameterName}.Value = material.GetVector({v.parameterName}.ParamName);");
                        }
                        else if (v.propertyType == MaterialPropertyType.Float)
                        {
                            sw.WriteLine($"{v.parameterName}.Value = material.GetFloat({v.parameterName}.ParamName);");
                        }
                        else if (v.propertyType == MaterialPropertyType.Int)
                        {
                            sw.WriteLine($"{v.parameterName}.Value = material.GetInt({v.parameterName}.ParamName);");
                        }
                    }
                    sw.WriteLine(endScope);

                    // write Deserialize function
                    sw.WriteLine($"public static async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache,{nameof(AsyncLoadTexture)} loadTexture, {nameof(AsyncLoadTexture)} loadNormalMap, {nameof(AsyncLoadCubemap)} loadCubemap)");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine("while (reader.Read())");
                        sw.WriteLine(startScope);
                        {
                            sw.WriteLine("if (reader.TokenType == JsonToken.PropertyName)");
                            sw.WriteLine(startScope);
                            {
                                sw.WriteLine("var curProp = reader.Value.ToString();");
                                sw.WriteLine("switch (curProp)");
                                sw.WriteLine(startScope);
                                {
                                    foreach (var v in properties)
                                    {
                                        sw.WriteLine($"case {className}.{v.constParameter}:");
                                        switch (v.propertyType)
                                        {
                                            case MaterialPropertyType.Texture2D:
                                                {
                                                    sw.WriteLine(startScope);
                                                    sw.WriteLine($"var texInfo = TextureInfo.Deserialize(root, reader);");
                                                    sw.WriteLine($"var tex = await loadTexture(texInfo.Index);");
                                                    sw.WriteLine($"matCache.SetTexture({className}.{v.constParameter}, tex);");
                                                    sw.WriteLine(endScope);
                                                    break;
                                                }
                                            case MaterialPropertyType.Cubemap:
                                                {
                                                    sw.WriteLine(startScope);
                                                    sw.WriteLine("reader.Read(); reader.Read();");
                                                    sw.WriteLine($"Cubemap cubemap = await loadCubemap(new CubemapId() {{ Id = reader.ReadAsInt32().Value,Root = root }});");
                                                    sw.WriteLine($"matCache.SetTexture({className}.{v.constParameter}, cubemap);");
                                                    sw.WriteLine(endScope);
                                                    break;
                                                }
                                            case MaterialPropertyType.Color:
                                                {
                                                    sw.WriteLine($"matCache.SetColor({className}.{v.constParameter}, reader.ReadAsRGBAColor().ToUnityColorRaw());");
                                                    break;
                                                }

                                            case MaterialPropertyType.Vector:
                                                {
                                                    sw.WriteLine($"matCache.SetVector({className}.{v.constParameter}, reader.ReadAsVector4().ToUnityVector4Raw());");
                                                    break;
                                                }
                                            case MaterialPropertyType.Float:
                                                {
                                                    sw.WriteLine($"matCache.SetFloat({className}.{v.constParameter}, reader.ReadAsFloat());");
                                                    break;
                                                }
                                            case MaterialPropertyType.Int:
                                                {
                                                    sw.WriteLine($"matCache.SetInt({className}.{v.constParameter}, reader.ReadAsInt32().Value);");
                                                    break;
                                                }
                                        }
                                        sw.WriteLine("break;");
                                    }
                                    sw.WriteLine("case nameof(keywords):");
                                    sw.WriteLine(startScope);
                                    {
                                        sw.WriteLine("var keywords = reader.ReadStringList();");
                                        sw.WriteLine("foreach (var keyword in keywords)");
                                        sw.WriteLine("matCache.EnableKeyword(keyword);");
                                    }
                                    sw.WriteLine(endScope);
                                    sw.WriteLine("break;");
                                }
                                sw.WriteLine(endScope);
                            }
                            sw.WriteLine(endScope);
                        }
                        sw.WriteLine(endScope);
                    }
                    sw.WriteLine(endScope);

                    // write Serialize function
                    sw.WriteLine("public override JProperty Serialize()");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine("JObject jo = new JObject();");
                        foreach (var v in properties)
                        {
                            switch (v.propertyType)
                            {
                                case MaterialPropertyType.Texture2D:
                                    sw.WriteLine($"if ({v.parameterName} != null && {v.parameterName}.Value != null) jo.Add({v.parameterName}.ParamName, {v.parameterName}.Serialize());");
                                    break;
                                case MaterialPropertyType.Cubemap:
                                    sw.WriteLine($"if ({v.parameterName} != null) jo.Add({v.parameterName}.ParamName, {v.parameterName}.Serialize());");
                                    break;
                                case MaterialPropertyType.Color:
                                    sw.WriteLine($"jo.Add({v.parameterName}.ParamName, {v.parameterName}.Value.ToNumericsColorRaw().ToJArray());");
                                    break;
                                case MaterialPropertyType.Vector:
                                    sw.WriteLine($"jo.Add({v.parameterName}.ParamName, {v.parameterName}.Value.ToGltfVector4Raw().ToJArray());");
                                    break;
                                case MaterialPropertyType.Float:
                                    sw.WriteLine($"jo.Add({v.parameterName}.ParamName, {v.parameterName}.Value);");
                                    break;
                                case MaterialPropertyType.Int:
                                    sw.WriteLine($"jo.Add({v.parameterName}.ParamName, {v.parameterName}.Value);");
                                    break;
                            }
                        }
                        sw.WriteLine("if(keywords != null && keywords.Length > 0)");
                        sw.WriteLine(startScope);
                        {
                            sw.WriteLine("JArray jKeywords = new JArray();");
                            sw.WriteLine("foreach (var keyword in jKeywords)");
                            sw.WriteLine("jKeywords.Add(keyword);");
                            sw.WriteLine("jo.Add(nameof(keywords), jKeywords);");
                        }
                        sw.WriteLine(endScope);
                        sw.WriteLine($"return new JProperty({className}.SHADER_NAME, jo);");
                    }
                    sw.WriteLine(endScope);
                }
                sw.WriteLine(endScope);
            }
            sw.WriteLine(endScope);
            // finish string write
            File.WriteAllText(exportPath, sw.ToString());
            AssetDatabase.ImportAsset(exportPath);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(exportPath));
        }
    }
}