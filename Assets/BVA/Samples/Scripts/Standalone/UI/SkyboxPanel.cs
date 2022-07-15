using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BVA.Sampler
{
    public class SkyboxPanel : MonoBehaviour
    {
        public Dropdown skyboxDropDown;
        Text label;
        public List<Material> materials;
        public void Awake()
        {
            label = skyboxDropDown.GetComponentInChildren<Text>();
            skyboxDropDown.ClearOptions();
            skyboxDropDown.onValueChanged.AddListener(OnSelected);
        }
        public void OnSelected(int index)
        {
            RenderSettings.skybox = materials[index];
            label.text = RenderSettings.skybox.name;
        }
        public void SetSkybox(SkyboxContainer container)
        {
            materials = container.materials;
            var options = new List<Dropdown.OptionData>();
            foreach (var m in materials)
            {
                if (m != null)
                    options.Add(new Dropdown.OptionData(m.name));
            }
            skyboxDropDown.options = options;
        }
    }
}