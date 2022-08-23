using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public class ComponentExtraAttribute : Attribute { }

    public interface IComponentExtra : IExtra, ICloneable
    {
        void Deserialize(GLTFRoot root, JsonReader reader, Component component);
        void SetData(Component component);
        string ComponentName { get; }
        Type ComponentType { get; }
    }
    public class AsyncComponentExtraAttribute : Attribute { }

    public interface IAsyncComponentExtra : IExtra, ICloneable
    {
        Task Deserialize(GLTFRoot root, JsonReader reader, Component component, AsyncLoadTexture loadTexture, AsyncLoadMaterial loadMaterial, AsyncLoadSprite loadSprite, AsyncLoadCubemap loadCubemap);
        void SetData(Component component, ExportTexture exportTexture, ExportMaterial exportMaterial, ExportSprite exportSprite, ExportCubemap exportCubemap);
        string ComponentName { get; }
        Type ComponentType { get; }
    }

    public static class ComponentImporter
    {
        private static Dictionary<string, IComponentExtra> customComponents;
        public static Dictionary<string, IComponentExtra> CustomComponents
        {
            get
            {
                if (customComponents == null)
                {
                    customComponents = new Dictionary<string, IComponentExtra>();
                    System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(ComponentExtraAttribute));
                    Type[] types = asm.GetExportedTypes();

                    Func<Attribute[], bool> IsAttribute = o =>
                    {
                        foreach (Attribute a in o)
                        {
                            if (a is ComponentExtraAttribute)
                                return true;
                        }
                        return false;
                    };

                    var cosType = types.Where(o =>
                    {
                        return IsAttribute(Attribute.GetCustomAttributes(o, true));
                    });
                    foreach (var extraType in cosType)
                    {
                        IComponentExtra extraExtra = (IComponentExtra)Activator.CreateInstance(extraType);
                        customComponents.Add(extraExtra.ComponentName, extraExtra);
                    }
                }
                return customComponents;
            }
        }
        private static Dictionary<string, IAsyncComponentExtra> customAsyncComponents;
        public static Dictionary<string, IAsyncComponentExtra> CustomAsyncComponents
        {
            get
            {
                if (customAsyncComponents == null)
                {
                    customAsyncComponents = new Dictionary<string, IAsyncComponentExtra>();
                    System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(AsyncComponentExtraAttribute));
                    Type[] types = asm.GetExportedTypes();

                    Func<Attribute[], bool> IsAttribute = o =>
                    {
                        foreach (Attribute a in o)
                        {
                            if (a is AsyncComponentExtraAttribute)
                                return true;
                        }
                        return false;
                    };

                    var cosType = types.Where(o =>
                    {
                        return IsAttribute(Attribute.GetCustomAttributes(o, true));
                    });
                    foreach (var extraType in cosType)
                    {
                        IAsyncComponentExtra extraExtra = (IAsyncComponentExtra)Activator.CreateInstance(extraType);
                        customAsyncComponents.Add(extraExtra.ComponentName, extraExtra);
                    }
                }
                return customAsyncComponents;
            }
        }
        public static void ExportComponentExtra(GameObject nodeObj, Node node, ExportTexture exportTexture, ExportMaterial exportMaterial, ExportSprite exportSprite, ExportCubemap exportCubemap)
        {
            foreach (var kvp in CustomComponents)
            {
                var component = nodeObj.GetComponent(kvp.Key);
                if (component != null)
                {
                    IComponentExtra componentExtra = kvp.Value.Clone() as IComponentExtra;
                    componentExtra.SetData(component);
                    node.AddExtra(kvp.Value.ComponentName, componentExtra);
                }
            }
            foreach (var kvp in CustomAsyncComponents)
            {
                var component = nodeObj.GetComponent(kvp.Key);
                if (component != null)
                {
                    IAsyncComponentExtra componentExtra = kvp.Value.Clone() as IAsyncComponentExtra;
                    componentExtra.SetData(component, exportTexture, exportMaterial, exportSprite, exportCubemap);
                    node.AddExtra(kvp.Value.ComponentName, componentExtra);
                }
            }
        }

        public static bool ImportComponent(string componentName, GLTFRoot root, JsonReader reader, GameObject gameObject, AsyncLoadTexture loadTexture, AsyncLoadMaterial loadMaterial, AsyncLoadSprite loadSprite, AsyncLoadCubemap loadCubemap)
        {
            if (CustomComponents.TryGetValue(componentName, out var extra))
            {
                var component = gameObject.GetComponent(extra.ComponentType) ?? gameObject.AddComponent(extra.ComponentType);
                if (component == null)
                {
                    Debug.LogError($"Gameobject {gameObject.name} doesn't have component {componentName}");
                    return false;
                }
                extra.Deserialize(root, reader, component);
                return true;
            }
            if (CustomAsyncComponents.TryGetValue(componentName, out var extra2))
            {
                var component = gameObject.GetComponent(extra2.ComponentType) ?? gameObject.AddComponent(extra2.ComponentType);
                if (component == null)
                {
                    Debug.LogError($"Gameobject {gameObject.name} doesn't have component {componentName}");
                    return false;
                }
                extra2.Deserialize(root, reader, component, loadTexture, loadMaterial, loadSprite, loadCubemap);
                return true;
            }
            return false;
        }
    }
}
