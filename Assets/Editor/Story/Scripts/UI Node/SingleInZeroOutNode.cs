using UnityEngine;

namespace Editor.Story
{
    public class SingleInZeroOutNode: BaseNode
    {
        public override void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            base.Init(graphView, title, position);
            
            Type = NodeType.SingleInZeroOut;
            ChoiceDatas.Clear();
        }

        public override void Draw()
        {
            DrawMainContainer();
            DrawTitleContainer();
            DrawTitleButtonContainer();
            DrawTopContainer();
            DrawInputContainer();
            DrawExtensionContainer();
        }
    }
}
