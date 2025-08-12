using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Story
{
    public class NodeCreationBox : ScriptableObject , ISearchWindowProvider
    {
        
        private StoryGraphView graphView;
        //缩进图标
        private Texture2D indentationIcon;
        
        public void Init(StoryGraphView graphView)
        {
            this.graphView = graphView;
            
            //设置缩进图标
            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }
        
        //创建搜索树
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("添加节点")),
                new SearchTreeEntry(new GUIContent("对话", indentationIcon))
                {
                    level = 1,
                    userData = NodeType.Dialogue
                },
                new SearchTreeEntry(new GUIContent("分支", indentationIcon))
                {
                    level = 1,
                    userData = NodeType.Branch
                },
                new SearchTreeEntry(new GUIContent("开始", indentationIcon))
                {
                    level = 1,
                    userData = NodeType.Start
                },
                new SearchTreeEntry(new GUIContent("结束", indentationIcon))
                {
                    level = 1,
                    userData = NodeType.End
                },
            };

            return searchTreeEntries;
        }

        //当点击某个对话框
        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {

            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition);
            
            NodeType type = (NodeType)SearchTreeEntry.userData;
            switch (type)
            {
                case NodeType.Dialogue:
                case NodeType.Branch:
                case NodeType.Start:
                case NodeType.End:
                    graphView.CreateNode(SearchTreeEntry.content.text , type ,localMousePosition);
                    return true;
                default:
                    return false;
            }
        }
    }
}
