using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nothke.ProtoGUI
{
    public abstract class WindowGUI : GameWindow
    {
        // Overridable fields:
        [HideInInspector] public Rect windowRect = new Rect(360, 20, 250, 0);
        protected bool draggable = true;
        protected float labelWidth = 100;
        protected float sliderNumberWidth = 60;

        // Properties
        public override Rect Rect => windowRect;

        public override bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        static GUISkin _skin;
        public static GUISkin skin
        {
            get
            {
                if (!_skin)
                    _skin = Resources.Load<GUISkin>("ProtoGUISkin");

                return _skin;
            }
        }

        int id;
        string windowTooltip;

        private void Awake()
        {
            id = Random.Range(int.MinValue, int.MaxValue);
        }

        protected virtual void OnGUI()
        {
            GUI.skin = skin;

            // Clamp to screen edges
            if (windowRect.x < 0) windowRect.x = 0;
            if (windowRect.y < 10) windowRect.y = 0;
            if (windowRect.xMax > Screen.width) windowRect.x = Screen.width - windowRect.width;
            if (windowRect.y > Screen.height - 35) windowRect.y = Screen.height - 35;

            windowRect = GUILayout.Window(id, windowRect, WindowBase, WindowLabel);

            if (!string.IsNullOrEmpty(windowTooltip))
            {
                Vector2 mouse = Input.mousePosition;
                mouse.y = Screen.height - mouse.y;
                //mouse -= windowRect.position;
                GUI.Label(new Rect(50, Screen.height - 80, 1000, 1000), windowTooltip);
            }
        }

        void WindowBase(int id)
        {
            Window();

            if (draggable)
                GUI.DragWindow();

            if (Event.current.type == EventType.Repaint)
                windowTooltip = GUI.tooltip;
        }

        // Extensions

        protected abstract void Window();

        protected void Foldout(string label, ref bool value, string tooltip = null)
        {
            bool clicked = string.IsNullOrEmpty(tooltip) ?
                Button((value ? "[-] " : "[+] ") + label) :
                Button((value ? "[-] " : "[+] ") + label, tooltip);

            if (clicked)
            {
                value = !value;
                RefreshHeight();
            }
        }

        public void RefreshHeight()
        {
            windowRect.height = 0;
        }

        #region Labels and fields

        protected void Label(string label)
        {
            GUILayout.Label(label);
        }

        protected void Label(string label, string tooltip)
        {
            GUILayout.Label(new GUIContent(label, tooltip));
        }

        protected void LabelField(string label, ref string value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            value = GUILayout.TextField(value);
            GUILayout.EndHorizontal();
        }

        protected string LabelField(string label, string value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            value = GUILayout.TextField(value);
            GUILayout.EndHorizontal();

            return value;
        }

        [System.Obsolete("Use non-ref return overload instead")]
        protected void FloatField(string label, ref float value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(labelWidth));
            value = float.Parse(GUILayout.TextField(value.ToString()));

            GUILayout.EndHorizontal();
        }

        protected float FloatField(string label, float value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(labelWidth));
            float prevValue = value;
            if (float.TryParse(GUILayout.TextField(value.ToString()), out float newValue))
                value = newValue;
            else value = prevValue;

            GUILayout.EndHorizontal();

            return value;
        }

        protected int IntField(string label, int value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(labelWidth));
            int prevValue = value;
            if (int.TryParse(GUILayout.TextField(value.ToString()), out int newValue))
                value = newValue;
            else value = prevValue;

            GUILayout.EndHorizontal();

            return value;
        }

        #endregion

        #region Sliders

        #region ref
        protected void Slider(string label, ref float value, float min, float max, string tooltip = null)
        {
            GUILayout.BeginHorizontal();

            TTip(label, tooltip);

            GUILayout.Label(value.ToString(), GUILayout.Width(sliderNumberWidth));
            value = GUILayout.HorizontalSlider(value, min, max);

            GUILayout.EndHorizontal();
        }

        protected void Slider(string label, ref int value, float min, float max, string tooltip = null)
        {
            GUILayout.BeginHorizontal();

            TTip(label, tooltip);

            GUILayout.Label(value.ToString(), GUILayout.Width(sliderNumberWidth));
            value = Mathf.RoundToInt(GUILayout.HorizontalSlider(value, min, max));

            GUILayout.EndHorizontal();
        }

        protected void Slider(string label, ref float value, float min, float max, float stepSize, string tooltip = null)
        {
            GUILayout.BeginHorizontal();

            TTip(label, tooltip);

            GUILayout.Label(value.ToString("F1"), GUILayout.Width(sliderNumberWidth));
            value = Mathf.Round(GUILayout.HorizontalSlider(value, min, max) / stepSize) * stepSize;

            GUILayout.EndHorizontal();
        }

        #endregion

        #region non-ref

        protected float Slider(string label, float value, float min, float max, in string format = null, string tooltip = null)
        {
            GUILayout.BeginHorizontal();

            TTip(label, tooltip);

            if (string.IsNullOrEmpty(format))
                GUILayout.Label(value.ToString(), GUILayout.Width(sliderNumberWidth));
            else
                GUILayout.Label(value.ToString(format), GUILayout.Width(sliderNumberWidth));
            value = GUILayout.HorizontalSlider(value, min, max);

            GUILayout.EndHorizontal();

            return value;
        }

        protected int Slider(string label, int value, float min, float max, string tooltip = null)
        {
            GUILayout.BeginHorizontal();

            TTip(label, tooltip);

            GUILayout.Label(value.ToString(), GUILayout.Width(sliderNumberWidth));
            value = Mathf.RoundToInt(GUILayout.HorizontalSlider(value, min, max));

            GUILayout.EndHorizontal();

            return value;
        }

        protected float Slider(string label, float value, float min, float max, float stepSize, string tooltip = null)
        {
            GUILayout.BeginHorizontal();

            TTip(label, tooltip);

            GUILayout.Label(value.ToString("F1"), GUILayout.Width(sliderNumberWidth));
            value = Mathf.Round(GUILayout.HorizontalSlider(value, min, max) / stepSize) * stepSize;

            GUILayout.EndHorizontal();

            return value;
        }
        #endregion

        #endregion

        #region Buttons

        protected bool Button(string label)
        {
            return GUILayout.Button(label);
        }

        protected bool Button(string label, string tooltip)
        {
            return GUILayout.Button(new GUIContent(label, tooltip));
        }

        protected bool ToggleButton(string name, bool value)
        {
            if (GUILayout.Button(name + (value ? " ON" : " OFF")))
                value = !value;

            return value;
        }

        protected bool ToggleButton(string name, ref bool value)
        {
            if (GUILayout.Button(name + (value ? " ON" : " OFF")))
            {
                value = !value;
                return true;
            }

            return false;
        }

        #endregion

        void TTip(in string label, in string tooltip)
        {
            if (string.IsNullOrEmpty(tooltip))
                GUILayout.Label(label, GUILayout.Width(labelWidth));
            else
                GUILayout.Label(new GUIContent(label, tooltip), GUILayout.Width(labelWidth));
        }
    }
}