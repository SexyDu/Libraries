using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SexyDu.UI
{
    public abstract class ButtonInteractColor : ButtonInteract
    {
        [SerializeField] protected Color[] cols_normal;
        [SerializeField] protected Color[] cols_press;
        
        public override void OnButtonPress()
        {
            SetRendersColor(cols_press);
        }

        public override void OnButtonUp()
        {
            SetRendersColor(cols_normal);
        }

        protected abstract void SetRendersColor(Color[] cols);

#if UNITY_EDITOR
        public abstract void SetDefaultColors();
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonInteractColor), true)]
    public class ButtonInteractColorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("SetDefaultColors"))
            {
                ButtonInteractColor comp = target as ButtonInteractColor;
                comp.SetDefaultColors();
            }
        }
    }
#endif
}