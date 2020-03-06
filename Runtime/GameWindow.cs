using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nothke.ProtoGUI
{
    public abstract class GameWindow : MonoBehaviour
    {

        public abstract string WindowLabel { get; }

        public abstract bool Enabled { get; set; }

        public abstract Rect Rect { get; }

        static List<GameWindow> all;

#if UNITY_2019_3_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void DomainReload()
        {
            if (all != null) all.Clear();
            all = null;
            areAllHidden = false;
        }
#endif

        public void OnEnable()
        {
            if (all == null) all = new List<GameWindow>();

            if (!all.Contains(this))
                all.Add(this);
        }

        bool wasEnabledBeforeHiding;

        public static bool areAllHidden;

        public static void HideAll(bool hide)
        {
            if (all == null) return;

            for (int i = 0; i < all.Count; i++)
            {
                if (hide)
                {
                    all[i].wasEnabledBeforeHiding = all[i].Enabled;

                    all[i].Enabled = false;
                }
                else
                {
                    if (all[i].wasEnabledBeforeHiding)
                    {
                        all[i].Enabled = true;
                        all[i].wasEnabledBeforeHiding = false;
                    }
                }
            }

            areAllHidden = hide;
        }

        public static bool IsMouseOverUI()
        {
            if (all == null || all.Count <= 0)
                return false;

            Vector2 mousePos = Input.mousePosition;
            mousePos.y = Screen.height - mousePos.y;

            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].Enabled && all[i].Rect.Contains(mousePos))
                    return true;
            }

            return false;
        }
    }
}