using System.Collections.Generic;
using UnityEngine;

namespace BVA.Component
{
    [RequireComponent(typeof(Renderer))]
    [AddComponentMenu("BVA/Variable/Material Variable Collection")]
    public class MaterialVariableCollection : VariableCollection<Material>
    {
        private void Awake()
        {
            defaultValue = GetComponent<Renderer>().material;
        }
        public void SetMaterial(int index)
        {
            GetComponent<Renderer>().material = variables[index];
        }
        public void SetMaterial(int slot,int index)
        {
            GetComponent<Renderer>().materials[slot] = variables[index];
        }
    }
}