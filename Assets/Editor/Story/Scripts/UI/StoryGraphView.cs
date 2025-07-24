using UnityEditor.Experimental.GraphView;
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
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //graphview窗口内容的拖动
            this.AddManipulator(new ContentDragger());
            //选中Node移动功能
            this.AddManipulator(new SelectionDragger());
            //多个node框选功能
            this.AddManipulator(new RectangleSelector());
        }
        
    }
}
