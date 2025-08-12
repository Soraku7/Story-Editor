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
        private NodeCreationBox nodeCreationBox;

        public StoryGraphView(StoryEditorWindow window)
        {
            //实例化时绑定窗口
            storyEditorWindow = window;
            
            AddGridBackground();
            AddManipulators();
            AddDefaultNode();
            AddNodeCreationBox();
            
            OnOpenNodeCreationBox();
        }
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            //添加右键菜单
            evt.menu.AppendAction("添加节点", (action) =>
            {
                //获取光标位置
               Vector2 screenMousePosition = action.eventInfo.mousePosition + new Vector2(50 , 35);
               
               //出发请求事件
               nodeCreationRequest(new NodeCreationContext()
               {
                   screenMousePosition = screenMousePosition,
                   index = -1
               });
            });
            
            
            
        }
        
        //添加网格背景
        private void AddGridBackground()
        {
            //创建网格背景
            GridBackground gridBackground = new();
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
            SetupZoom(0.2f , 2.0f);
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
           CreateNode("开始" , NodeType.Start , Vector2.zero);
           CreateNode("结束" , NodeType.End , new Vector2(500, 0));
        }
        
        //添加节点创建框
        private void AddNodeCreationBox()
        {
            nodeCreationBox = ScriptableObject.CreateInstance<NodeCreationBox>();
            nodeCreationBox.Init(this);
        }

        //打开添加节点对话框
        private void OnOpenNodeCreationBox()
        {
            //定义请求事件
            nodeCreationRequest = context =>
            {
                //打开节点创建框
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), nodeCreationBox);
            };
        }
        
        public Vector2 GetLocalMousePosition(Vector2 screenMousePosition)
        {
            //将光标的屏幕坐标转换为窗口内的坐标
            Vector2 windowMousePosition = screenMousePosition - storyEditorWindow.position.position;

            //将光标在当前窗口内的坐标转换为节点视图内的坐标
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);
            
            return localMousePosition;
        }
    }
}
