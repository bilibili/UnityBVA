using UnityEditor;
using UnityEngine;
using BVA.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

namespace BVA
{
    public partial class PostProcessExtraGenerator : EditorWindow
    {

        [MenuItem("BVA/Developer Tools/Code-Gen/PostProcess")]
        static void Init()
        {
            PostProcessExtraGenerator window = (PostProcessExtraGenerator)EditorWindow.GetWindow(typeof(PostProcessExtraGenerator), false, "Export Scene Settings");
            window.Show();
        }

        MonoScript monoBehaviourScript;
        const string DEFAULT_SCRIPT_PATH = "Assets/BVA/Runtime/Scripts/BVA/GenerateScriptFloder/";
        string exportPath = DEFAULT_SCRIPT_PATH;
        void OnGUI()
        {
            monoBehaviourScript = EditorGUILayout.ObjectField("MonoBehaviour Script ", monoBehaviourScript, typeof(MonoScript), false) as MonoScript;
            if (monoBehaviourScript == null) return;
            EditorGUILayout.TextField($"ClassName : {monoBehaviourScript.name}");

            var monoClass = monoBehaviourScript.GetClass();

            var monoFields = GetLegelFieldInfo(monoClass);//OYM:get legal field 

            foreach (var fieldinfo in monoFields)
            {
                System.Type fieldType = fieldinfo.FieldType;
                var name = fieldinfo.Name;
                // var desc = monoProperties[i].;
                EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField($"Desc : {desc}");
                EditorGUILayout.LabelField($"Name : {name}");
                EditorGUILayout.LabelField($"Type : { fieldType.FullName}");
                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.BeginHorizontal();
            exportPath = EditorGUILayout.TextField("Path : ", exportPath);
            if (GUILayout.Button("Choose", GUILayout.MaxWidth(100)))
            {
                exportPath = EditorUtility.SaveFilePanelInProject("Choose Script Path", $"{monoClass.Name}", "cs", "save", DEFAULT_SCRIPT_PATH);
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Generator Script"))
            {
                ExportScript(monoClass, monoFields);
            }
        }

        public static IEnumerable<FieldInfo> GetLegelFieldInfo(Type targetType)
        {
            var allField = targetType.GetTypeInfo().DeclaredFields;
            return allField.Where((a) => { return a.IsPublic; });
        }

        private void ExportScript(Type monoClass, IEnumerable<FieldInfo> monoFields)
        {

            string[] namespaceField = new string[] { "Newtonsoft.Json.Linq", "GLTF.Math" , "GLTF.Schema", "Newtonsoft.Json" , "GLTF.Extensions", "BVA.Extensions",  "UnityEngine",
"Color = UnityEngine.Color","Vector4 = UnityEngine.Vector4" };
            const string namespaceHeader = "namespace GLTF.Schema.BVA";
            const string startScope = "{", endScope = "}";
            string className = Path.GetFileNameWithoutExtension(exportPath);
            string classHeader = $"public class {className} : PostProcessBase";

            string scriptType = monoClass.FullName;
            if (string.IsNullOrWhiteSpace(className))
            {
                EditorUtility.DisplayDialog("error", "need file name,and should start with BVA!", "OK");
            }
            StringWriter sw = new StringWriter();
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
                    //sw.WriteLine($"public const string PROPERTY = \"{className}\";");
                    #region FieldName 
                    //write parameter declaration
                    foreach (var fieldInfo in monoFields)
                    {
                        sw.WriteLine($"public {fieldInfo.FieldType} {fieldInfo.Name } ;");
                    }
                    #endregion

                    #region Construct
                    //write construction function
                    sw.WriteLine($"public {className}()");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine($"type =PostProcessType.{className}; ");
                        sw.WriteLine($"active =true; ");
                    }
                    sw.WriteLine(endScope);
                    sw.WriteLine();
                    /*                    sw.WriteLine($"public {className}({scriptType} target)");
                                        sw.WriteLine(startScope);
                                        {
                                           // sw.WriteLine($"{scriptType} targetComponent =target as {scriptType} ;");
                                            foreach (var fieldInfo in monoFields)
                                            {
                                                sw.WriteLine($"this.{fieldInfo.Name} =target.{fieldInfo.Name};");
                                            }
                                        }
                                        sw.WriteLine(endScope);*/
                    #endregion

                    #region From 

                    sw.WriteLine($"public static {className} From({scriptType} target)");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine($" {className} result =new {className}();");

                        sw.WriteLine($" result.type =PostProcessType.{className};");
                        sw.WriteLine($" result.active =target.active;");
                        //write parameter declaration
                        foreach (var fieldInfo in monoFields)
                        {
                            sw.WriteLine($"result. {fieldInfo.Name} =target.{fieldInfo.Name }.value ;");
                        }
                        sw.WriteLine($" return result ;");
                    }

                    sw.WriteLine(endScope);

                    #endregion

                    #region SetData 

                    sw.WriteLine($"public override void SetData(UnityEngine.Rendering.VolumeComponent component)");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine($"base.SetData(component);");
                        sw.WriteLine($"  {scriptType} result =component as  {scriptType};");
                        //write parameter declaration
                        foreach (var fieldInfo in monoFields)
                        {
                            sw.WriteLine($"result. {fieldInfo.Name}.value ={fieldInfo.Name } ;");
                        }
                    }
                    sw.WriteLine(endScope);

                    #endregion

                    #region DeSerialize
                    //write Deserialize function
                    sw.WriteLine($"public static {className} Deserialize(GLTFRoot root, JObject jo)");

                    sw.WriteLine(startScope);
                    {

                        sw.WriteLine($"{ className} result =new { className}(); ");
                        sw.WriteLine($"JToken jt;");
                        sw.WriteLine("jt = jo.GetValue(nameof(result.active)); ");
                        sw.WriteLine(" if (jt != null) result.active = jt.DeserializeAsBool();");
                        foreach (var fieldInfo in monoFields)
                        {
                            sw.WriteLine($" jt = jo.GetValue(nameof(result.{fieldInfo.Name})); ");
                            sw.WriteLine($" if (jt != null)result.{fieldInfo.Name} = jt.; ");
                        }
                        sw.WriteLine($" return result ;");
                    }
                    sw.WriteLine(endScope);


                    #endregion

                    #region Serialize
                    //write Serialize function
                    //write Serialize function
                    sw.WriteLine("public override JObject Serialize()");
                    sw.WriteLine(startScope);
                    {
                        sw.WriteLine("JObject pro =  base.Serialize();");
                        foreach (var fieldInfo in monoFields)
                        {
                            sw.WriteLine($"pro.Add(new JProperty(nameof({fieldInfo.Name}), {fieldInfo.Name}.));");
                        }
                        sw.WriteLine($"return pro;");
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