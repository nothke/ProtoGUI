using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;

namespace Nothke.ProtoGUI
{
    public abstract class WindowUI : GameWindow
    {
        public override bool Enabled
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }

        RectTransform _rectTransform;
        RectTransform rectTransform { get { if (!_rectTransform) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }

        public override Rect Rect => RectTransformToScreenSpace(rectTransform);

        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
            rect.x -= (transform.pivot.x * size.x);
            rect.y -= ((1.0f - transform.pivot.y) * size.y);
            return rect;
        }
    }
}