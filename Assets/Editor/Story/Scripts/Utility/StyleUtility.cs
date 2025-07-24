using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public static class StyleUtility
    {
        /// <summary>
        /// 添加类名
        /// </summary>
        /// <param name="element"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static VisualElement AddClasses(this VisualElement element , params string[] className)
        {
            foreach (var item in className)
            {
                element.AddToClassList(item);
            }
            return element;
        }

        /// <summary>
        /// 添加样式表
        /// </summary>
        /// <param name="element"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static VisualElement AddStyleSheet(this VisualElement element , params string[] filePath)
        {
            foreach (var item in filePath)
            {
                //载入文件
                StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(item);
                //添加引用
                element.styleSheets.Add(styleSheet);
            }

            return element;
        }
        
    }
}
