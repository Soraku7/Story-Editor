using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public class SingleInMultiOutNode : BaseNode
    {
        public override void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            base.Init(graphView, title, position);
            Type = NodeType.SingleInMultiOut;
            
            ChoiceDatas.Clear();
            ChoiceDatas.Add(new("选项文本"));
        }

        protected override void DrawOutputContainer()
        {
            for(int i = 0 ; i < ChoiceDatas.Count; i++)
            {
                ChoiceData choiceData = ChoiceDatas[i];
                output = CreateOutputPort(choiceData);
                outputContainer.Add(output);
            }
        }

        protected override void DrawExtensionContainer()
        {
            Button btnAdd = ElementUtility.CreateButton("添加选项", () =>
            {
                ChoiceData newChoiceData = new("新选项");
                ChoiceDatas.Add(newChoiceData);
                
                output = CreateOutputPort(newChoiceData);
                outputContainer.Add(output);
            });
            
            extensionContainer.Add(btnAdd);
            
            RefreshExpandedState();
        }

        private Port CreateOutputPort(object userData)
        {
            //获取选择数据
            ChoiceData choiceData = (ChoiceData)userData;

            Port outputPort = this.CreatePort();
            
            //创建删除按钮
            Button btnDelete = ElementUtility.CreateButton("X", () =>
            {
                if (ChoiceDatas.Count == 1)
                {
                    Debug.LogWarning("至少要保留一条选项");
                    return;
                }
                
                ChoiceDatas.Remove(choiceData);
                graphView.RemoveElement(outputPort);
                
            });
            
            //创建文本选项框
            TextField tfdText = ElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });
            
            outputPort.Add(btnDelete);
            outputPort.Add(tfdText);
            
            return outputPort;
        }
    }
}
