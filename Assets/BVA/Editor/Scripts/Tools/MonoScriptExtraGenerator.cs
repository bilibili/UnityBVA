using UnityEditor;
using UnityEngine;
using BVA.Extensions;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Object = UnityEngine.Object;
using GLTF.Schema.BVA;
using System.Reflection;

namespace BVA
{
    public partial class MonoScriptExtraGenerator : EditorWindow
    {

        [MenuItem("BVA/Developer Tools/Generate Script From GameObject")]
        static void Init()
        {
            MonoScriptExtraGenerator window = (MonoScriptExtraGenerator)EditorWindow.GetWindow(typeof(MonoScriptExtraGenerator), false, "Export Component as Extra");
            window.Show();
        }

        Object targetObject;
        Object lastTargetObject;

        Dictionary<Type, MemberInfoExtra[]> detectClassAndMembers;
        Dictionary<Type, bool> isFoldOutMember;
        const string DEFAULT_SCRIPT_PATH = "/BVA/Runtime/Scripts/BVA/Runtime";
        static string exportPath;
        static string DefaultExportPath => UnityTools.GetAssetPath() + DEFAULT_SCRIPT_PATH;
        Vector2 scrollPosition;
        bool includeBaseType;
        private void OnEnable()
        {
            if (exportPath == null)
                exportPath = DefaultExportPath;
            detectClassAndMembers = new Dictionary<Type, MemberInfoExtra[]>();
            isFoldOutMember = new Dictionary<Type, bool>();
        }

        void OnGUI()
        {
            void detectChange()
            {
                detectClassAndMembers.Clear();
                isFoldOutMember.Clear();
                if (targetObject is GameObject)
                {
                    var detectType = (targetObject as GameObject).GetComponents<UnityEngine.Component>().Select(a => a.GetType());
                    foreach (var item in detectType)
                    {
                        if (!detectClassAndMembers.TryGetValue(item, out _))
                        {
                            detectClassAndMembers.Add(item, GetLegelMemberInfo(item, includeBaseType));
                            isFoldOutMember.Add(item, true);
                        }
                    }
                    lastTargetObject = targetObject;
                }
                else if (targetObject is MonoScript)
                {
                    var key = (targetObject as MonoScript).GetClass();
                    detectClassAndMembers.Add(key, GetLegelMemberInfo(key, includeBaseType));
                    isFoldOutMember.Add(key, true);
                    lastTargetObject = targetObject;
                }
                else
                {
                    var key = targetObject.GetType();
                    detectClassAndMembers.Add(key, GetLegelMemberInfo(key, includeBaseType));
                    isFoldOutMember.Add(key, true);
                    lastTargetObject = targetObject;
                }
            }
            EditorGUILayout.BeginHorizontal();
            includeBaseType = EditorGUILayout.Toggle("Include Base", includeBaseType);
            if (GUILayout.Button("Refresh"))
                detectChange();
            EditorGUILayout.EndHorizontal();
            targetObject = EditorGUILayout.ObjectField("MonoBehaviour Script or GameObject", targetObject, typeof(Object), true) as Object;

            if (targetObject == null) return;
            else if (lastTargetObject != targetObject)
            {
                detectChange();
            }
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var classMembersPair in detectClassAndMembers)
            {
                var legalFields = classMembersPair.Value;
                var monoClass = classMembersPair.Key;
                EditorGUILayout.Space(10);
                isFoldOutMember[monoClass] = EditorGUILayout.Foldout(isFoldOutMember[monoClass], $"ClassName : {monoClass.Name}");
                EditorGUILayout.Space(10);
                if (isFoldOutMember[monoClass])
                {
                    for (int i = 0; i < legalFields.Length; i++)
                    {
                        Type fieldType = legalFields[i].MemberClassType;
                        Attribute[] attribute = Attribute.GetCustomAttributes(fieldType, typeof(Attribute));
                        var name = legalFields[i].MemberName;
                        // var desc = monoProperties[i].;
                        EditorGUILayout.BeginHorizontal();
                        legalFields[i].IsGenerate = EditorGUILayout.ToggleLeft($"Name : {name}", legalFields[i].IsGenerate);
                        EditorGUILayout.LabelField($"Member Type: {legalFields[i].MemberTypeName}");
                        EditorGUILayout.LabelField($"Member ClassType : {fieldType.FullName.Replace('+', '.')}");
                        EditorGUILayout.EndHorizontal();

                    }
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(10);
                exportPath = EditorGUILayout.TextField("Path : ", exportPath);
                if (GUILayout.Button("Choose", GUILayout.MaxWidth(100)))
                {
                    exportPath = EditorUtility.SaveFilePanelInProject("Choose Script Path", $"BVA_{monoClass.Name}_Extra", "cs", "save");
                    if (string.IsNullOrEmpty(exportPath))
                        exportPath = DefaultExportPath;
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Generate Script"))
                {
                    ExportScript(monoClass, legalFields.Where(a => a.IsGenerate));
                }
            }
            EditorGUILayout.EndScrollView();
        }
        private string GetDeserializeFunctionName(string typename) => $"Deserialize_{typename}";

        private void ExportScript(Type monoClass, IEnumerable<MemberInfoExtra> monoFields)
        {
            bool hasMaterial = monoFields.Any(monoField => monoField.IsMaterial);
            bool hasTexture = monoFields.Any(monoField => monoField.IsTexture);
            bool hasSprite = monoFields.Any(monoField => monoField.IsSprite);
            string[] namespaceField = new string[] { "Newtonsoft.Json.Linq", "GLTF.Math", "GLTF.Schema", "Newtonsoft.Json", "GLTF.Extensions", "BVA.Extensions", "BVA.Component", "System.Threading.Tasks", "UnityEngine", "Color = UnityEngine.Color", "Vector4 = UnityEngine.Vector4" };
            const string namespaceHeader = "namespace GLTF.Schema.BVA";
            const string startScope = "{", endScope = "}";
            string className = Path.GetFileNameWithoutExtension(exportPath);
            string classHeader = $"public class {className} : IExtra";
            string scriptType = monoClass.FullName;
            List<MemberInfoExtra> innerSerializableClass = new List<MemberInfoExtra>();
            if (string.IsNullOrWhiteSpace(className))
            {
                EditorUtility.DisplayDialog("error", "need file name,and should start with BVA!", "OK");
            }
            StringWriter sw = new StringWriter();
            void SerializeInnerMemeber(IEnumerable<MemberInfoExtra> innerSerializableClass)
            {
                foreach (var fieldInfo in innerSerializableClass)
                {
                    string typename = fieldInfo.MemberClassType.Name;
                    string variableName = "j_input";
                    sw.WriteLine($"public static JObject Serialize_{typename}({typename} input)");
                    sw.WriteLine(startScope);
                    {
                        var serializableFieldTypes = GetLegelMemberInfo(fieldInfo.MemberClassType, true);

                        sw.WriteLine($"JObject {variableName} = new JObject();");
                        foreach (var v in serializableFieldTypes)
                        {
                            if (TypeValueSerializeFuncDic.TryGetValue(v.MemberClassType, out var innerFunc))
                            {
                                string member = $"input.{v.MemberName}";
                                sw.WriteLine($"{variableName}.Add(nameof({member}), {innerFunc(member)});");
                            }
                        }
                    }
                    sw.WriteLine($"return {variableName};");
                    sw.WriteLine(endScope);
                }
            }
            void DeserializeInnerMemeber(IEnumerable<MemberInfoExtra> innerSerializableClass)
            {
                foreach (var fieldInfo in innerSerializableClass)
                {
                    string typename = fieldInfo.MemberClassType.Name;
                    sw.WriteLine($"public static {typename} Deserialize_{typename}(GLTFRoot root, JsonReader reader)");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine($"{typename} target = new {typename}();");
                        sw.WriteLine("reader.Read();");
                        sw.WriteLine("while (reader.Read() && reader.TokenType == JsonToken.PropertyName)");
                        sw.WriteLine(startScope);
                        {
                            sw.WriteLine("var curProp = reader.Value.ToString();");
                            sw.WriteLine("switch (curProp)");
                            sw.WriteLine(startScope);
                            {
                                var serializableFieldTypes = GetLegelMemberInfo(fieldInfo.MemberClassType, true);
                                foreach (var v in serializableFieldTypes)
                                {
                                    ReadFieldInfo(v);
                                    //sw.WriteLine($"case nameof(target.{v.Name}):");
                                    //WriteInnerField(sw, v);
                                    //sw.WriteLine($"break;");
                                }
                            }
                            sw.WriteLine(endScope);
                        }
                        sw.WriteLine(endScope);
                    }
                    sw.WriteLine($"return target;");
                    sw.WriteLine(endScope);
                }
            }
            void ReadFieldInfo(MemberInfoExtra fieldInfo)
            {
                sw.WriteLine($"case nameof(target.{fieldInfo.MemberName}):");
                if (fieldInfo.IsMaterial)
                {
                    sw.WriteLine($"int materialIndex = reader.ReadAsInt32().Value;\n\ttarget.{fieldInfo.MemberName} = await {FUNCTOIN_LOAD_MATERIAL}(new MaterialId() {{ Id = materialIndex, Root = root }});");
                }
                else if (fieldInfo.IsTexture)
                {
                    sw.WriteLine($"int textureIndex = reader.ReadAsInt32().Value;\n\ttarget.{fieldInfo.MemberName} = await {FUNCTION_LOAD_TEXTURE}(new TextureId() {{ Id = textureIndex, Root = root }});");
                }
                else if (fieldInfo.IsSprite)
                {
                    sw.WriteLine($"int spriteIndex = reader.ReadAsInt32().Value;\n\ttarget.{fieldInfo.MemberName} = await {FUNCTION_LOAD_SPRITE}(new SpriteId() {{ Id = spriteIndex, Root = root }});");
                }
                else if (JsonReaderDeSerializeFuncDic.TryGetValue(fieldInfo.MemberClassType, out var Func))
                {
                    sw.WriteLine($"target.{fieldInfo.MemberName} = {Func(fieldInfo.MemberClassType)}");
                }
                else if (fieldInfo.MemberClassType.IsEnum)
                {
                    sw.WriteLine($"target.{fieldInfo.MemberName} = reader.ReadStringEnum<{fieldInfo.MemberClassType.FullName.Replace('+', '.')}>();");
                }
                else if (fieldInfo.IsList)
                {
                    if (ListSerializeFuncDic.TryGetValue(fieldInfo.GenericType, out var FuncReadList))
                    {
                        sw.WriteLine($"target.{fieldInfo.MemberName} = {FuncReadList("reader")};");
                    }
                    else
                    {
                        string elementTypeName = fieldInfo.GenericType;

                        Type elementType = Type.GetType(elementTypeName);
                        var findedClass = innerSerializableClass.Find(x => { Type t = x.MemberClassType.Assembly.GetType(elementTypeName); return t != null && t == x.MemberClassType; });
                        if (findedClass != null)
                        {
                            sw.WriteLine($"target.{fieldInfo.MemberName} = reader.ReadList(()=>Deserialize_{findedClass.MemberClassType}(root,reader));");
                        }
                    }
                }
                else if (fieldInfo.IsArray)
                {
                    if (ListSerializeFuncDic.TryGetValue(fieldInfo.ArrayType, out var FuncReadArray))
                    {
                        sw.WriteLine($"target.{fieldInfo.MemberName} = {FuncReadArray("reader")}.ToArray();");
                    }
                    else
                    {
                        string elementTypeName = fieldInfo.ArrayType;

                        Type elementType = Type.GetType(elementTypeName);
                        var findedClass = innerSerializableClass.Find(x => { Type t = x.MemberClassType.Assembly.GetType(elementTypeName); return t != null && t == x.MemberClassType; });
                        if (findedClass != null)
                        {
                            sw.WriteLine($"target.{fieldInfo.MemberName} = reader.ReadList(()=>Deserialize_{findedClass.MemberClassType}(root,reader)).ToArray();");
                        }
                    }
                }
                else if (fieldInfo.IsSerializableClass || fieldInfo.IsStruct)
                {
                    sw.WriteLine($"target.{fieldInfo.MemberName} = {GetDeserializeFunctionName(fieldInfo.MemberClassType.Name)}(root, reader);");
                    innerSerializableClass.Add(fieldInfo);
                }
                sw.WriteLine("break;");
            }
            #region Using
            foreach (var f in namespaceField)
            {
                sw.WriteLine($"using {f};");
            }
            #endregion

            sw.WriteLine();
            sw.WriteLine(namespaceHeader);
            sw.WriteLine(startScope);
            {
                sw.WriteLine(classHeader);
                sw.WriteLine(startScope);
                {
                    sw.WriteLine($"public const string PROPERTY = \"{className}\";");
                    #region FieldName 
                    //write parameter declaration
                    foreach (var fieldInfo in monoFields)
                    {
                        string classType = fieldInfo.MemberClassType.ToString().Replace('+', '.');
                        if (classType.Contains("List`"))
                        {
                            classType = classType.Replace("List`1[", "List<").Replace("]", ">");
                        }
                        string baseClassType = classType;
                        if (fieldInfo.MemberClassType.IsArray)
                            baseClassType = fieldInfo.ArrayType;
                        if (fieldInfo.MemberClassType.IsGenericType)
                            baseClassType = fieldInfo.GenericType;
                        Debug.Log(baseClassType);
                        if (BaseTypeMapper.ContainsKey(baseClassType))
                        {
                            classType = classType.Replace(baseClassType, BaseTypeMapper[baseClassType]);
                        }
                        if (fieldInfo.IsMaterial)
                            sw.WriteLine($"public MaterialId {fieldInfo.MemberName};");
                        else if (fieldInfo.IsTexture)
                            sw.WriteLine($"public TextureId {fieldInfo.MemberName};");
                        else if (fieldInfo.IsSprite)
                            sw.WriteLine($"public SpriteId {fieldInfo.MemberName};");
                        else
                            sw.WriteLine($"public {classType} {fieldInfo.MemberName};");
                    }
                    #endregion

                    #region Construct
                    //write construction function
                    sw.WriteLine($"public {className}(){{}}");
                    sw.WriteLine();
                    sw.Write($"public {className}({scriptType} target");
                    if (hasMaterial) sw.Write($",MaterialId materialId=null");
                    if (hasTexture) sw.Write($",TextureId textureId=null");
                    if (hasSprite) sw.Write($",SpriteId spriteId=null");
                    sw.Write(")");
                    sw.WriteLine(startScope);
                    {
                        foreach (var fieldInfo in monoFields)
                        {
                            if (fieldInfo.IsMaterial)
                                sw.WriteLine($"this.{fieldInfo.MemberName} = materialId;");
                            else if (fieldInfo.IsTexture)
                                sw.WriteLine($"this.{fieldInfo.MemberName} = textureId;");
                            else if (fieldInfo.IsSprite)
                                sw.WriteLine($"this.{fieldInfo.MemberName} = spriteId;");
                            else
                                sw.WriteLine($"this.{fieldInfo.MemberName} = target.{fieldInfo.MemberName};");
                        }
                    }
                    sw.WriteLine(endScope);
                    #endregion
                    bool isTaskMethod = hasMaterial || hasTexture || hasSprite;

                    #region Deserialize
                    //write Deserialize function
                    if (isTaskMethod)
                        sw.WriteLine($"public static async Task Deserialize(GLTFRoot root, JsonReader reader, {scriptType}  target, {nameof(AsyncLoadTexture)} {FUNCTION_LOAD_TEXTURE}=null,{nameof(AsyncLoadMaterial)} {FUNCTOIN_LOAD_MATERIAL}=null, {nameof(AsyncLoadSprite)} {FUNCTION_LOAD_SPRITE}=null)");
                    else
                        sw.WriteLine($"public static void Deserialize(GLTFRoot root, JsonReader reader, {scriptType}  target)");
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
                                    foreach (var fieldInfo in monoFields)
                                    {
                                        ReadFieldInfo(fieldInfo);
                                    }
                                }
                                sw.WriteLine(endScope);
                            }
                            sw.WriteLine(endScope);
                        }
                        sw.WriteLine(endScope);
                    }
                    sw.WriteLine(endScope);

                    #endregion

                    #region DeserializeSerializableType
                    DeserializeInnerMemeber(innerSerializableClass);
                    SerializeInnerMemeber(innerSerializableClass);
                    #endregion

                    #region Serialize
                    //write Serialize function
                    sw.WriteLine("public JProperty Serialize()");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine("JObject jo = new JObject();");
                        foreach (var fieldInfo in monoFields)
                        {
                            if (fieldInfo.IsTexture || fieldInfo.IsSprite || fieldInfo.IsMaterial)
                            {
                                sw.WriteLine($"if({fieldInfo.MemberName}!=null)jo.Add(nameof({fieldInfo.MemberName}), {fieldInfo.MemberName});");
                            }
                            else if (TypeValueSerializeFuncDic.TryGetValue(fieldInfo.MemberClassType, out var Func))
                            {
                                sw.WriteLine($"jo.Add(nameof({fieldInfo.MemberName}), {Func(fieldInfo.MemberName)});");
                            }
                            else if (fieldInfo.MemberClassType.IsEnum)
                            {
                                sw.WriteLine($"jo.Add(nameof({fieldInfo.MemberName}), {fieldInfo.MemberName}.ToString());");
                            }
                            else if (fieldInfo.IsList || fieldInfo.IsArray)
                            {
                                string elementTypeName = fieldInfo.ArrayType;
                                if (string.IsNullOrEmpty(elementTypeName))
                                    elementTypeName = fieldInfo.GenericType;

                                Type elementType = Type.GetType(elementTypeName);
                                sw.WriteLine($"JArray j_{fieldInfo.MemberName} = new JArray();");
                                sw.WriteLine($"foreach(var item in {fieldInfo.MemberName})");
                                if (elementType != null && TypeValueSerializeFuncDic.TryGetValue(elementType, out var serializeFunc))
                                {
                                    sw.WriteLine($"j_{fieldInfo.MemberName}.Add({serializeFunc("item")});");
                                }
                                else
                                {
                                    var findedClass = innerSerializableClass.Find(x => { Type t = x.MemberClassType.Assembly.GetType(elementTypeName); return t != null && t == x.MemberClassType; });
                                    if (findedClass != null)
                                    {
                                        sw.WriteLine($"j_{fieldInfo.MemberName}.Add(Serialize_{findedClass.MemberClassType}(item));");
                                    }
                                }
                                //sw.WriteLine($"  j_{fieldInfo.MemberName}.Add(item);");
                                sw.WriteLine($"jo.Add(nameof({fieldInfo.MemberName}), j_{fieldInfo.MemberName});");
                            }
                            else if (fieldInfo.IsStruct || fieldInfo.IsSerializableClass)
                            {
                                var serializableFieldTypes = GetLegelMemberInfo(fieldInfo.MemberClassType, true);
                                string variableName = $"j_{fieldInfo.MemberName}";
                                sw.WriteLine($"JObject {variableName} = Serialize_{fieldInfo.MemberClassType}({fieldInfo.MemberName});");
                                sw.WriteLine($"jo.Add(nameof({fieldInfo.MemberName}), {variableName});");
                            }
                        }
                        sw.WriteLine($"return new JProperty({className}.PROPERTY, jo);");
                    }
                    sw.WriteLine(endScope);
                    #endregion
                }
                sw.WriteLine(endScope);
            }
            sw.WriteLine(endScope);
            //finish string write
            File.WriteAllText(exportPath, sw.ToString());
            AssetDatabase.ImportAsset(exportPath);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(exportPath));
        }

        string ParameterName(string desc) { return "parameter_" + desc.GetNumberAlphaUnderLine(); }
    }
}