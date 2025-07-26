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
            
            //添加右键菜单
            evt.menu.AppendAction("添加默认节点", (a) =>
            {
                AddElement(CreateNode("默认节点", Vector2.zero));
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
        public BaseNode CreateNode(string title, Vector2 position)
        {
            //创建节点
            BaseNode node = new BaseNode();
            //初始化节点
            node.Init(this, title, position);
            node.Draw();
            return node;
        }

        //添加默认节点
        public void AddDefaultNode()
        {
            AddElement(CreateNode("默认节点", Vector2.zero));
        }

    }
}
