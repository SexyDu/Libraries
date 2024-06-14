using System.Collections.Generic;

namespace SexyDu.UI
{
    public class ButtonInteractMessenger : ButtonInteract
    {
        public override void OnButtonPress()
        {
            for (int i = 0; i < interacts.Count; i++)
            {
                interacts[i].OnButtonPress();
            }
        }

        public override void OnButtonUp()
        {
            for (int i = 0; i < interacts.Count; i++)
            {
                interacts[i].OnButtonUp();
            }
        }

        private List<IButtonInteract> interacts = new List<IButtonInteract>();

        #region Statics
        public static ButtonInteractMessenger operator +(ButtonInteractMessenger target, IButtonInteract interact)
        {
            if (!target.interacts.Contains(interact))
                target.interacts.Add(interact);

            return target;
        }
        public static ButtonInteractMessenger operator -(ButtonInteractMessenger target, IButtonInteract interact)
        {
            target.interacts.Remove(interact);

            return target;
        }
        #endregion
    }
}