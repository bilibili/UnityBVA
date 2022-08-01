using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BVA.Sample
{
    public class CameraPanel : MonoBehaviour
    {
        Camera[] allCameras;
        Text label;
        public Dropdown cameraDropDown;
        public void OnSelected(int index)
        {
            SetActiveCamera(allCameras[index]);
            label.text = allCameras[index].name;
        }
        void Awake()
        {
            label = cameraDropDown.GetComponentInChildren<Text>();
            allCameras = Camera.allCameras;
            cameraDropDown.onValueChanged.AddListener(OnSelected);
            //SetActiveCamera(Camera.current);
        }
        void SetActiveCamera(Camera camera)
        {
            foreach (var cam in allCameras)
            {
                cam.gameObject.SetActive(cam == camera);
            }
        }
        public void SetCameras()
        {
            allCameras = Camera.allCameras;
            var options = new List<Dropdown.OptionData>();
            foreach (var m in allCameras)
            {
                options.Add(new Dropdown.OptionData(m.name));
            }
            cameraDropDown.options = options;
        }
    }
}