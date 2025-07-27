using UnityEngine;

namespace Editor.Story
{
    public class SingleInSingleOutNode : BaseNode
    {
        public override void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            base.Init(graphView, title, position);
            Type = NodeType.SingleInSingleOut;
        }
    }
}
