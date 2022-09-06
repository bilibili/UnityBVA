using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using GLTF.Schema;
using GLTF.Schema.BVA;

namespace BVA
{
    public partial class MonoScriptExtraGenerator
    {
        const string FUNCTOIN_LOAD_MATERIAL = "loadMaterial";
        const string FUNCTION_LOAD_TEXTURE = "loadTexture";
        const string FUNCTION_LOAD_SPRITE = "loadSprite";
        const string FUNCTION_LOAD_CUBEMAP = "loadCubemap";

        public readonly static Dictionary<Type, Func<string, string>> TypeValueSerializeFuncDic = new Dictionary<Type, Func<string, string>>()
        {
            {typeof(bool), (a)=>$"{a}" },
            {typeof(byte), (a)=>$"{a}" },
            {typeof(short), (a)=>$"{a}"},
            {typeof(ushort), (a)=>$"{a}"},
            {typeof(int), (a)=>$"{a}"},
            {typeof(string), (a)=>$"{a}"},
            {typeof(float), (a)=>$"{a}"},
            {typeof(double), (a)=>$"{a}"},
            {typeof(decimal), (a)=>$"{a}"},
            {typeof(LayerMask), (a)=>$"{a}.value"},
            {typeof(Vector2), (a)=> $"{a}.ToJArray()"},
            {typeof(Vector3), (a)=>$"{a}.ToJArray()" },
            {typeof(Vector4), (a)=> $"{a}.ToJArray()"},
            {typeof(Rect), (a)=> $"{a}.ToJArray()"},
            {typeof(Quaternion), (a)=> $"{a}.ToJArray()"},
            {typeof(Color), (a)=> $"{a}.ToJArray()"},
            {typeof(Texture), (a)=> $"{a}.Id"},
            {typeof(Material), (a)=> $"{a}.Id"},
            {typeof(Sprite), (a)=> $"{a}.Id"},
            {typeof(AnimationCurve), (a)=>$"distanceAttenuationCurve.Serialize()"}
        };
        public readonly static Dictionary<Type, Func<string, string>> TypeValueSerializeFuncDic2 = new Dictionary<Type, Func<string, string>>()
        {
            {typeof(MaterialId), (a)=>$"{a}.Id"},
            {typeof(TextureId), (a)=>$"{a}.Id"},
            {typeof(SpriteId), (a)=>$"{a}.Id"},
        };

        public readonly static Dictionary<string, string> BaseTypeMapper = new Dictionary<string, string>()
        {
            {"System.Single","float"},
            {"System.Double","double"},
            {"System.Int32","int"},
            {"System.Int64","long"},
            {"System.String","string"},
            {"System.Boolean","bool"}
        };

        public readonly static Dictionary<string, Func<string, string>> ListSerializeFuncDic = new Dictionary<string, Func<string, string>>()
        {
            {"System.String",(a)=>$"{a}.ReadStringList()"},
            {"System.Single",(a)=>$"{a}.ReadFloatList()"},
            {"System.Double",(a)=>$"{a}.ReadDoubleList()"},
            {"System.Int32",(a)=>$"{a}.ReadInt32List()"},
            {"System.Int64",(a)=>$"{a}.ReadInt64List()"},
            {"System.Boolean",(a)=>$"{a}.ReadBoolList()"}
        };
        public readonly static Dictionary<Type, Func<Type, string>> JsonReaderDeSerializeFuncDic = new Dictionary<Type, Func<Type, string>>()
        {
            {typeof(bool), (a)=> " reader.ReadAsBoolean().Value;"},
            {typeof(byte), (a)=> " reader.ReadAsBytes()[0];"},

            {typeof(short), (a)=>" (short)reader.ReadAsInt32().Value;"},
            {typeof(ushort), (a)=> " (ushort)reader.ReadAsInt32().Value;"},

            {typeof(int), (a)=>  " reader.ReadAsInt32().Value;"},
            {typeof(uint), (a)=> " (uint)reader.ReadAsInt32().Value;"},

            {typeof(string), (a)=> " reader.ReadAsString();"},

            {typeof(float), (a)=> " reader.ReadAsFloat();"},
            {typeof(double), (a)=> " reader.ReadAsDouble().Value;"},
            {typeof(decimal), (a)=>  " reader.ReadAsDecimal().Value;"},
            {typeof(LayerMask), (a)=> " reader.ReadAsInt32().Value;"},
            {typeof(Vector2), (a)=>  " reader.ReadAsVector2();"},
            {typeof(Vector3), (a)=>  " reader.ReadAsVector3();"},
            {typeof(Vector4), (a)=> $" reader.ReadAsVector4();"},
            {typeof(Rect), (a)=> $" reader.ReadAsVector4().ToUnityRectRaw();"},
            {typeof(Quaternion), (a)=> $" reader.ReadAsQuaternion();"},
            {typeof(Color), (a)=> $" reader.ReadAsRGBAColor();"},
            {typeof(AnimationCurve), (a)=> $"AnimationCurveExtension.DeserializeAnimationCurve(root,reader);" }
    };

        public static MemberInfoExtra[] GetLegelMemberInfo(Type targetType, bool includeBaseType)
        {
            var allField = targetType.GetTypeInfo().DeclaredFields.ToList();
            if (includeBaseType)
            {
                Type baseType = targetType.GetTypeInfo().BaseType;
                while (baseType != null)
                {
                    allField.AddRange(baseType.GetTypeInfo().DeclaredFields);
                    baseType = baseType.GetTypeInfo().BaseType;
                }
            }
            allField = allField.Where((a) =>
            {
                return
                a.IsPublic &&
                !a.IsStatic &&
                (a.FieldType.IsEnum || a.FieldType.IsSerializable || TypeValueSerializeFuncDic.ContainsKey(a.FieldType));
            }).ToList();

            var allProperties = targetType.GetTypeInfo().DeclaredProperties.ToList();
            if (includeBaseType)
            {
                Type baseType = targetType.GetTypeInfo().BaseType;
                while (baseType != null)
                {
                    allProperties.AddRange(baseType.GetTypeInfo().DeclaredProperties);
                    baseType = baseType.GetTypeInfo().BaseType;
                }
            }

            allProperties = allProperties.Where((a) =>
            {
                return
              a.CanRead &&
              a.CanWrite &&
              a.GetCustomAttribute<ObsoleteAttribute>() == null &&
            (a.PropertyType.IsEnum || TypeValueSerializeFuncDic.ContainsKey(a.PropertyType));
            }).ToList();

            var MemberInfos = Enumerable.Concat(allField.Select(a => new MemberInfoExtra(a.Name, a.MemberType.ToString(), a.FieldType)),
                allProperties.Select(a => new MemberInfoExtra(a.Name, a.MemberType.ToString(), a.PropertyType))).ToArray();
            return MemberInfos;
        }

        public enum MemberType
        {
            Generic,
            MaterialId,
        }

        public class MemberInfoExtra
        {
            public MemberInfoExtra(string name, string typeName, Type type)
            {
                MemberName = name;
                MemberTypeName = typeName;
                MemberClassType = type;
            }

            public string MemberName { get; }
            public string MemberTypeName { get; }
            public string GenericType => IsList ? MemberClassType.ToString().Replace("System.Collections.Generic.List`1[", string.Empty).Replace("]", string.Empty) : null;
            public string ArrayType => IsArray ? MemberClassType.ToString().Replace("[]", string.Empty) : null;
            public Type MemberClassType { get; }

            public bool IsGenerate = true;
            public bool IsStruct => !MemberClassType.IsPrimitive && !MemberClassType.IsEnum && MemberClassType.IsValueType;
            public bool IsSerializableClass => (MemberClassType.IsGenericType || MemberClassType.IsArray || MemberClassType.IsAbstract) == false && MemberClassType.IsClass && MemberClassType.IsSerializable;
            public bool IsArray => MemberClassType.IsArray;
            public bool IsList => MemberClassType.IsGenericType && MemberClassType.FullName.Contains("List");
            public bool IsMaterial => MemberClassType == typeof(Material);
            public bool IsTexture => MemberClassType == typeof(Texture);
            public bool IsSprite => MemberClassType == typeof(Sprite);
            public bool IsCubemap => MemberClassType == typeof(Cubemap);
        }
    };

}