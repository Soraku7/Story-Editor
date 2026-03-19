using System.Collections.Generic;
using UnityEngine;

namespace Editor.Story
{
    public class StoryDataSO : ScriptableObject
    {
        [SerializeField] private string fileName;
        [SerializeField] private List<GroupData> groupDatas;
        [SerializeField] private List<NodeData> nodeDatas;
        
        public string FileName { get => fileName; set => fileName = value; }
        public List<GroupData> GroupDatas { get => groupDatas; set => groupDatas = value; }
        public List<NodeData> NodeDatas { get => nodeDatas; set => nodeDatas = value; }

        public void Init(string fileName)
        {
            this.fileName = fileName;
            groupDatas = new List<GroupData>();
            nodeDatas = new List<NodeData>();
        }
    }
}