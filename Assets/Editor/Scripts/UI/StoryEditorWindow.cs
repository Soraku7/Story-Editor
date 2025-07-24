using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Scripts.UI
{
    public class StoryEditorWindow : EditorWindow
    {
        [MenuItem("Window/StoryEditorWindow %&S")] // Ctrl + Alt + S
        public static void ShowExample()
        {
            StoryEditorWindow wnd = GetWindow<StoryEditorWindow>();
            wnd.titleContent = new GUIContent("StoryEditorWindow");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

        }
    }
}
