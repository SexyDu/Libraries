using UnityEngine;
using UnityEditor;

namespace SexyDu.UI
{
    [CustomEditor(typeof(ButtonHandler), true)]
    [CanEditMultipleObjects]
    public class ButtonHandlerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("SetInteracts"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    ButtonHandler comp = targets[i] as ButtonHandler;
                    comp.SetInteracts();
                }
            }
        }
    }
}