using UnityEngine;
using UnityEditor;

namespace SexyDu.UI
{
    [CustomEditor(typeof(ButtonInteract), true)]
    [CanEditMultipleObjects]
    public class ButtonInteractInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);
            GUIStyle style = new GUIStyle();
            style.fontSize = 15;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
            GUILayout.Label("Editor", style);
            if (GUILayout.Button("Construct Default Setting"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    ButtonInteract comp = targets[i] as ButtonInteract;
                    comp.ConstructDefaultSetting();

                    EditorUtility.SetDirty(comp);
                }
            }
        }
    }
}