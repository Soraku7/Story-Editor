using UnityEngine;

namespace Editor.Story
{
    public class StartNode : ZeroInSingleOutNode
    {
        public override void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            base.Init(graphView, title, position);
            Type = NodeType.Start;
        }
    }
}