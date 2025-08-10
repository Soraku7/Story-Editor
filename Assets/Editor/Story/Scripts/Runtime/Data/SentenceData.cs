using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Story
{
    [Serializable]
    public class SentenceData
    {
        [SerializeField]private string text;
        
        public string Text {get => text; set => text = value; }
        
        public SentenceData(string text)
        {
            this.text = text;
        }
    }
}