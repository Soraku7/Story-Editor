using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public class StoryGraphView : GraphView
    {
        //关联窗口
        private StoryEditorWindow storyEditorWindow;

        public StoryGraphView(StoryEditorWindow window)
        {
            //实例化时绑定窗口
            storyEditorWindow = window;
            
            AddGridBackground();
            AddManipulators();
            AddDefaultNode();
        }
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            Vector2 localMousePosition = this.contentViewContainer.WorldToLocal(evt.mousePosition);
            //添加右键菜单
            evt.menu.AppendAction("添加默认节点", (a) =>
            {
               CreateNode("默认节点", NodeType.Base , localMousePosition);
            });
            evt.menu.AppendAction("添加单进单出节点", (a) =>
            {
                CreateNode("1t1", NodeType.SingleInSingleOut ,localMousePosition);
            });
            evt.menu.AppendAction("添加单进多出节点", (a) =>
            {
                CreateNode("1tm", NodeType.SingleInMultiOut , localMousePosition);
            });
            evt.menu.AppendAction("添加单进0出节点", (a) =>
            {
                CreateNode("1t0", NodeType.SingleInZeroOut , localMousePosition);
            });
            evt.menu.AppendAction("添加0进1出节点", (a) =>
            {
                CreateNode("0t1", NodeType.ZeroInSingleOut , localMousePosition);
            });
            
            
            evt.menu.AppendAction("添加对话节点", (a) =>
            {
                CreateNode("对话节点", NodeType.Dialogue , localMousePosition);
            });
            evt.menu.AppendAction("添加分支节点", (a) =>
            {
                CreateNode("分支节点", NodeType.Branch , localMousePosition);
            });
            evt.menu.AppendAction("添加开始节点", (a) =>
            {
                CreateNode("开始节点", NodeType.Branch , localMousePosition);
            });
            evt.menu.AppendAction("添加结束节点", (a) =>
            {
                CreateNode("结束节点", NodeType.End , localMousePosition);
            });
            
        }
        
        //添加网格背景
        private void AddGridBackground()
        {
            //创建网格背景
            GridBackground gridBackground = new GridBackground();
            //设置网格背景拉伸与视图相同
            gridBackground.StretchToParentSize();
            //添加到GraphView
            Insert(0, gridBackground);
        }

        //添加试图操作
        private void AddManipulators()
        {
            //添加视图缩放
            // this.AddManipulators(new ContentZoomer());
            //滚轮缩放
            SetupZoom(0.2f , 4.0f);
            // SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //graphview窗口内容的拖动
            this.AddManipulator(new ContentDragger());
            //选中Node移动功能
            this.AddManipulator(new SelectionDragger());
            //多个node框选功能
            this.AddManipulator(new RectangleSelector());
        }
        
        //创建节点
        public BaseNode CreateNode(string title, NodeType type , Vector2 position)
        {
            //获取节点类型
            Type nodeType = Type.GetType("Editor.Story." + type + "Node");
            
            Debug.Log(type);
            //创建节点
            BaseNode node = Activator.CreateInstance(nodeType) as BaseNode;
            //初始化节点
            node.Init(this, title, position);
            node.Draw();
            AddElement(node);
            
            return node;
        }

        //添加默认节点
        public void AddDefaultNode()
        {
           CreateNode("节点" , NodeType.Base , Vector2.zero);
        }

    }
}
