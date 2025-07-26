using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Story
{
    // 基类节点
    public class BaseNode : Node
    {
        // UI元素
        protected StoryGraphView graphView;

        // 初始化
        public void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            this.graphView = graphView;
            this.title = title;
            SetPosition(new Rect(position, Vector2.zero));
        }
        
        // 绘制视图
        public virtual void Draw()
        {
            DrawMainContainer();
            DrawTitleContainer();
            DrawTitleButtonContainer();
            DrawTopContainer();
            DrawInputContainer();
            DrawOutputContainer();
            DrawExtensionContainer();
        }

        // 绘制主容器
        protected virtual void DrawMainContainer()
        {
        }

        // 绘制标题容器
        protected virtual void DrawTitleContainer()
        {
        }

        // 绘制标题按钮容器
        protected virtual void DrawTitleButtonContainer()
        {
        }

        // 绘制顶部容器
        protected virtual void DrawTopContainer()
        {
        }

        // 绘制输入容器
        protected virtual void DrawInputContainer() 
        {
        }

        // 绘制输出容器
        protected virtual void DrawOutputContainer()
        { 
        }

        // 绘制扩展容器
        protected virtual void DrawExtensionContainer() 
        {
        }
    }
}