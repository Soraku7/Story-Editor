using System;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public static class ElementUtility
    {
        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onClick"></param>
        /// <returns></returns>
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };
            return button;
        }

        /// <summary>
        /// 创建折叠组
        /// </summary>
        /// <param name="title"></param>
        /// <param name="collapsed"></param>
        /// <returns></returns>
        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        /// <summary>
        /// 创建单行文本输入框
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="onValueChanged"></param>
        /// <returns></returns>
        public static TextField CreateTextField(string value = null, string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        /// <summary>
        /// 创建多行文本输入框
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="onValueChanged"></param>
        /// <returns></returns>
        public static TextField CreateTextArea(string value = null, string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }
    }
}
