using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

        private Port CreateOutputPort(object userData)
        {
            ChoiceData choiceData = (ChoiceData)userData;

            Port outputPort = this.CreatePort();
            
            return outputPort;
        }
    }
}
