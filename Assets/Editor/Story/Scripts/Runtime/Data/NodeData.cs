using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.Story
{
    [Serializable]
    public class NodeData
    {
        [SerializeField] private string guid;
        [SerializeField] private NodeType type;
        [SerializeField] private Vector2 position;

        [SerializeField] private string title;
        [SerializeField] private string note;
        [SerializeField] private List<ChoiceData> choiceDatas;
        [SerializeField] private string groupID;

        [SerializeField] private string roleName;
        [SerializeField] public List<SentenceData> sentenceDatas;
        
        public string GUID { get => guid; set => guid = value; }
        public NodeType Type { get => type; set => type = value; }
        public Vector2 Position { get => position; set => position = value; }
        
        public string Title { get => title; set => title = value; }
        public string Note { get => note; set => note = value; }
        public List<ChoiceData> ChoiceDatas { get => choiceDatas; set => choiceDatas = value; }
        public string GroupID { get => groupID; set => groupID = value; }
        
        public string RoleName { get => roleName; set => roleName = value; }
    }
}