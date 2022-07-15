using System.Collections.Generic;
using UnityEngine;

namespace BVA.Component
{
    public enum VariableCollectionType
    {
        Mesh,
        Material,
        Texture,
        Audio,
        Animation,
        PostProcess,

    }
    [DisallowMultipleComponent]
    public class VariableCollection<T> : MonoBehaviour
    {
        public T defaultValue;
        public List<T> variables;
    }
}
