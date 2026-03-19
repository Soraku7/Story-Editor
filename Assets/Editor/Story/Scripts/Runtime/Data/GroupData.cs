using System;
using UnityEngine;

namespace Editor.Story
{
    [Serializable]
    public class GroupData
    {
        [SerializeField] private string title;
        [SerializeField] private string guid;
        [SerializeField] private Vector2 position;
        
        public string Title { get => title; set => title = value; }
        public string GUID { get => guid; set => guid = value; }
        public Vector2 Position { get => position; set => position = value; }
    }
}