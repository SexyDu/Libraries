using UnityEngine;

namespace SexyDu.UI.UGUI
{
    public class SafeAreaAnchor : SafeArea
    {
        public override void Set()
        {
            if (HasTarget)
            {
                var safeArea = Screen.safeArea;

                var minAnchor = safeArea.position;
                var maxAnchor = minAnchor + safeArea.size;

                minAnchor.x /= Screen.width;
                minAnchor.y /= Screen.height;
                maxAnchor.x /= Screen.width;
                maxAnchor.y /= Screen.height;

                target.anchorMin = minAnchor;
                target.anchorMax = maxAnchor;
            }
            else
                OccurNullTargetException();
        }
    }
}