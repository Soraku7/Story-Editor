using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public class StoryEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "StoryName";
        private readonly string variablePath ="Assets/Editor/Story/Style Sheets/Variables.uss";
        private readonly string toolbarStylePath = "Assets/Editor/Story/Style Sheets/ToolbarStyle.uss";
        private readonly string graphViewStylePath = "Assets/Editor/Story/Style Sheets/GraphViewStyle.uss";
        
        private StoryGraphView graphView;
        
        private Toolbar toolbar;
        private static TextField tfdFielName;
        private Button btnSave;
        private Button btnOpen;
        private Button btnNew;
        private Button btnClear;
        private Button btnMiniMap;
        
        
        [MenuItem("Tools/StoryEditorWindow %&S")] // Ctrl + Alt + S
        public static void ShowExample()
        {
            //获取窗口
            StoryEditorWindow wnd = GetWindow<StoryEditorWindow>();
            //窗口标题
            wnd.titleContent = new GUIContent("StoryEditorWindow");
        }

        public void CreateGUI()
        {
            AddToolbar();

            AddGraphView();
            
            AddStyles();
        }

        //添加工具栏
        private void AddToolbar()
        {
            //创建UI元素
            tfdFielName = ElementUtility.CreateTextField(defaultFileName , "新故事" , null);
            btnSave = ElementUtility.CreateButton("保存", null);
            btnOpen = ElementUtility.CreateButton("打开", null);
            btnNew = ElementUtility.CreateButton("新建", null);
            btnClear = ElementUtility.CreateButton("清空", null);
            btnMiniMap = ElementUtility.CreateButton("小地图", null);

            //创建工具栏
            toolbar = new();
            
            //添加UI元素到工具栏
            toolbar.Add(tfdFielName);
            toolbar.Add(btnSave);
            toolbar.Add(btnOpen);
            toolbar.Add(btnNew);
            toolbar.Add(btnClear);
            toolbar.Add(btnMiniMap);
            
            //工具栏加入到窗口
            rootVisualElement.Add(toolbar);
        }

        private void AddGraphView()
        {
            graphView = new StoryGraphView(this);
            
            // //将尺寸拉至与窗口相同
            // graphView.StretchToParentSize();
            // //将视图放入窗口中
            // rootVisualElement.Insert(0, graphView);
            rootVisualElement.Add(graphView);
        }

        //添加样式文件
        private void AddStyles()
        {
            //引用变量样式文件
            rootVisualElement.AddStyleSheet(variablePath);
            toolbar.AddStyleSheet(toolbarStylePath);
            graphView.AddStyleSheet(graphViewStylePath);
        }
    }
}
