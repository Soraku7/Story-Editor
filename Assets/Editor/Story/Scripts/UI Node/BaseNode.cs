using System.Collections.Generic;
using System.Linq;
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

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("清除输入连接",
                (action) => DisconnectedInputPorts(),
                HasAnyConnection()
                    ? DropdownMenuAction.Status.Normal
                    : DropdownMenuAction.Status.Disabled);

            evt.menu.AppendAction("清除输出连接",
                (action) => DisconnectedOutputPorts(),
                HasAnyConnection()
                    ? DropdownMenuAction.Status.Normal
                    : DropdownMenuAction.Status.Disabled);

            evt.menu.AppendAction("清除所有连接",
                (action) => DisconnectAllPorts(),
                HasAnyConnection()
                    ? DropdownMenuAction.Status.Normal
                    : DropdownMenuAction.Status.Disabled);

            evt.menu.AppendSeparator();
        }

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
            ChoiceDatas = new List<ChoiceData>() { new("下个节点") };

            //USS类名
            mainContainer.AddToClassList("node__main-container");
            titleContainer.AddToClassList("node__title-container");
            inputContainer.AddToClassList("node__input-container");
            outputContainer.AddToClassList("node__output-container");
            extensionContainer.AddToClassList("node__extension-container");
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
            titleContainer.Insert(0, tfdTitle);

            tfdTitle.AddClasses(
                "textfield",
                "textfield__hidden",
                "textfield__node-title");
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
            input = this.CreatePort("上个节点", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
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

            customDataContainer.AddClasses("node__custom-data-container");
            tfdNode.AddClasses("textfield", "textfield__quote", "foldout-item");

            foldout.Add(tfdNode);
            customDataContainer.Add(foldout);
            extensionContainer.Add(customDataContainer);

            //刷新
            RefreshExpandedState();
        }

        public void DisconnectAllPorts()
        {
            DisconnectedInputPorts();
            DisconnectedOutputPorts();
        }

        private void DisconnectedInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectedOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        //判断是否有任何连接
        public bool HasAnyConnection()
        {
            return HasInputConnection() || HasOutputConnection();
        }

        //判断是否有上行连接
        public bool HasInputConnection()
        {
            if (inputContainer.childCount == 0)
            {
                return false;
            }

            Port port = inputContainer.Children().First() as Port;

            if (port == null) return false;
            return port.connected;
        }

        //判断是否有下行连接
        public bool HasOutputConnection()
        {
            if (outputContainer.childCount == 0)
            {
                return false;
            }

            foreach (var visualElement in outputContainer.Children().ToList())
            {
                var port = (Port)visualElement;
                if (port.connected)
                {
                    return true;
                }
            }

            return false;
        }

        //断开目标端口
        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                DisconnectPort(port);
            }
        }

        protected void DisconnectPort(Port port)
        {
            if (port.connected)
            {
                graphView.DeleteElements(port.connections.ToList());
            }
        }
    }
}