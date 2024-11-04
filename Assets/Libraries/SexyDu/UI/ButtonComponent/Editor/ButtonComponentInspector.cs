using UnityEngine;
using UnityEditor;

namespace SexyDu.UI
{
    [CustomEditor(typeof(ButtonComponent), true)]
    [CanEditMultipleObjects]
    public class ButtonComponentInspector : ButtonHandlerInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("SetColliderComponent"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    ButtonComponent comp = targets[i] as ButtonComponent;
                    comp.SetColliderComponent();

                    EditorUtility.SetDirty(targets[i]);
                }
            }

            if (GUILayout.Button("SetEmployees"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    ButtonComponent comp = targets[i] as ButtonComponent;
                    comp.SetEmployees();

                    EditorUtility.SetDirty(targets[i]);
                }
            }
        }
    }
}