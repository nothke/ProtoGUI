using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nothke.ProtoGUI
{
    public class GUIToggler : MonoBehaviour
    {
        public KeyCode onKey = KeyCode.BackQuote;

        bool on = true;

        void Update()
        {
            if (Input.GetKeyDown(onKey))
            {
                on = !on;
                GameWindow.HideAll(!on);
            }
        }
    }
}