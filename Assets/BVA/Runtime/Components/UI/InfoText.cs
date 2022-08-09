using UnityEngine;

namespace BVA.Component
{
    /// <summary>
    /// Displays information text at the top left of the screen.
    /// </summary>
    public class InfoText : MonoBehaviour
    {
        [Tooltip("The information text to display")]
        [TextArea(20, 20)]
        public string info = "";

        protected virtual void OnGUI()
        {
            Rect rect = new Rect(0, 0, Screen.width / 2, Screen.height);

            GUI.Label(rect, info);
        }
    }
}