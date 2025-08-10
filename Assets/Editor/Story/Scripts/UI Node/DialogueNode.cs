using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

namespace Editor.Story
{
    public class DialogueNode : SingleInSingleOutNode
    {
        //角色名称
        public string RoleName { get; set; }
        
        public List<SentenceData> SentenceDatas { get; set; }

        public override void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            base.Init(graphView, title, position);
            Type = NodeType.Dialogue;
            
            RoleName = "角色名称";
            SentenceDatas = new List<SentenceData>()
            {
                new SentenceData("发言内容")
            };
        }
        

        protected override void DrawExtensionContainer()
        {
            customDataContainer = new VisualElement();
            foldout = ElementUtility.CreateFoldout("节点内容");
            
            //创建角色信息
            VisualElement roleInfoRowContainer = new VisualElement();
            VisualElement roleInfoColContainer = new VisualElement();
            
            TextField tfdRoleName = ElementUtility.CreateTextField(RoleName , null , callback =>
            {
                RoleName = callback.newValue;
            });
            
            roleInfoColContainer.Add(tfdRoleName);
            roleInfoRowContainer.Add(roleInfoColContainer);
            foldout.Add(roleInfoRowContainer);
            
            //创建添加按钮
            Button btnAdd = ElementUtility.CreateButton("添加句子", () =>
            {
                
                SentenceData newSentenceData = new SentenceData("新句子");
                SentenceDatas.Add(newSentenceData);
                
                VisualElement lineElement = CreateSentenceData(newSentenceData);
                foldout.Add(lineElement);
            });
            
            foldout.Add(btnAdd);
            customDataContainer.Add(foldout);
            extensionContainer.Add(customDataContainer);

            foreach (var sentenceData in SentenceDatas)
            {
                VisualElement lineElement = CreateSentenceData(sentenceData);
                foldout.Add(lineElement);
            }
            
            //刷新
            RefreshExpandedState();
        }

        private VisualElement CreateSentenceData(object userData)
        {
            //获取句子数据
            SentenceData sentenceData = (SentenceData)userData;
            
            //创建行元素
            VisualElement lineElement = new VisualElement();
            lineElement.userData = sentenceData;
            
            TextField tfdSentence = ElementUtility.CreateTextField(sentenceData.Text, null, callback =>
            {
                sentenceData.Text = callback.newValue;
            });
            
            Button btnDelete = ElementUtility.CreateButton("X", () =>
            {
                                
                if(SentenceDatas.Count == 1)
                {
                    Debug.LogWarning("至少有一条句子");
                    return;
                }
                
                SentenceDatas.Remove(sentenceData);
                foldout.Remove(lineElement);
            });
            
            lineElement.Add(tfdSentence);
            lineElement.Add(btnDelete);
            
            return lineElement;
        }
    }
}
