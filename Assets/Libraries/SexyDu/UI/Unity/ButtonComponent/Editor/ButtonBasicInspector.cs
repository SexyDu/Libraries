using UnityEngine;
using UnityEditor;

namespace SexyDu.UI.Unity
{
    [CustomEditor(typeof(ButtonBasic), true)]
    [CanEditMultipleObjects]
    public class ButtonBasicInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("SetInteracts"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    ButtonBasic comp = targets[i] as ButtonBasic;
                    comp.SetInteracts();
                }
            }
        }
    }
}