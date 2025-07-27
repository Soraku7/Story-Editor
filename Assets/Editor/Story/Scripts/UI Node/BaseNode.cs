using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Story
{
    // 基类节点
    public class BaseNode : Node
    {
        // UI元素
        protected StoryGraphView graphView;
        protected VisualElement customDataContainer;
        protected Foldout foldout;
        protected Port input;
        protected Port output;
        
        // 节点标题
        public string GUID { get; private set; }
        public NodeType Type { get; set; }
        public string Title { get; private set; }
        public string Note { get; private set; }
        
        //数据列表
        public List<ChoiceData> ChoiceDatas { get; set; }

        // 初始化
        public virtual void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            this.graphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
            
            //设置默认初始属性
            Type = NodeType.Base;
            GUID = UnityEditor.GUID.Generate().ToString();
            Title = title;
            Note = "备注信息";
            ChoiceDatas = new List<ChoiceData>(){new("下个节点")};
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
            TextField tfdTitle = ElementUtility.CreateTextField(Title, null, callback =>
            {
                // 更新标题
                Title = callback.newValue;
            });
            // 放入标题输入框
            titleContainer.Insert(0 , tfdTitle);
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
            input = this.CreatePort("上个节点", Orientation.Horizontal , Direction.Input , Port.Capacity.Multi);
            inputContainer.Add(input);
        }

        // 绘制输出容器
        protected virtual void DrawOutputContainer()
        {
            for (int i = 0; i < ChoiceDatas.Count; i++)
            {
                ChoiceData choiceData = ChoiceDatas[i];
                output = this.CreatePort(choiceData.Text);
                output.userData = choiceData;
                outputContainer.Add(output);
            }
        }

        // 绘制扩展容器
        protected virtual void DrawExtensionContainer()
        {
            //创建自定义容器
            customDataContainer = new VisualElement();
            
            //创建折叠组
            foldout = ElementUtility.CreateFoldout("节点内容");
            
            //创建备注输入框
            TextField tfdNode = ElementUtility.CreateTextArea(Note, null, callback =>
            {
                // 更新标题
                Note = callback.newValue;
            });

            
            foldout.Add(tfdNode);
            customDataContainer.Add(foldout);
            extensionContainer.Add(customDataContainer);
            
            //刷新
            RefreshExpandedState();
        }
    }
}