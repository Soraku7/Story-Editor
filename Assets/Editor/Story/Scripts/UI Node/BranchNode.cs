using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public class BranchNode: SingleInMultiOutNode
    {
        public override void Init(StoryGraphView graphView, string title, Vector2 position)
        {
            base.Init(graphView, title, position);
            Type = NodeType.Branch;
            
            ChoiceDatas.Clear();
            ChoiceDatas.Add(new ("选项文本1"));
            ChoiceDatas.Add(new ("选项文本2"));
        }

        protected override void DrawOutputContainer()
        {
            for (int i = 0; i < ChoiceDatas.Count; i++)
            {
                ChoiceData choiceData = ChoiceDatas[i];
                output = this.CreatePort(choiceData.Text);
                output.userData = choiceData;
                outputContainer.Add(output);
            }
        }

        protected override void DrawExtensionContainer()
        {
            customDataContainer = new VisualElement();
            
            foldout = ElementUtility.CreateFoldout("节点内容"); 
            //创建添加按钮
            Button btnAdd = ElementUtility.CreateButton("添加选项", () =>
            {
                ChoiceData newChoiceData = new("新选项");
                ChoiceDatas.Add(newChoiceData);
                
                VisualElement lineContainer = CreateChoiceData(newChoiceData);
                foldout.Add(lineContainer);
                
                OnAddChoiceData(newChoiceData);
                
            });
            
            foldout.Add(btnAdd);
            customDataContainer.Add(foldout);
            extensionContainer.Add(customDataContainer);

            foreach (var choiceData in ChoiceDatas)
            {
                VisualElement lineElement = CreateChoiceData(choiceData);
                foldout.Add(lineElement);
            }
            
            RefreshExpandedState();
        }

        private VisualElement CreateChoiceData(object userData)
        {
            ChoiceData choiceData = (ChoiceData)userData;
            
            VisualElement choiceContainer = new VisualElement();
            
            VisualElement lineElement = new VisualElement();
            
            lineElement.userData = choiceData;
            TextField tfdChoice = ElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
                OnEditChoiceData(choiceData);
            });
            
            Button btnDelete = ElementUtility.CreateButton("X", () =>
            {
                if (ChoiceDatas.Count == 1)
                {
                    Debug.LogWarning("至少要保留一条选项");
                    return;
                }
                
                ChoiceDatas.Remove(choiceData);
                foldout.Remove(lineElement);
                
                OnDeleteChoiceData(choiceData);
            });
            
            lineElement.Add(tfdChoice);
            lineElement.Add(btnDelete);
            choiceContainer.Add(lineElement);
            
            return choiceContainer;
        }

        //当编辑文本时
        private void OnEditChoiceData(ChoiceData choiceData)
        {
            //遍历所有端口
            foreach (Port port in outputContainer.Children())
            {
                if (port.userData == choiceData)
                {
                    port.name = choiceData.Text;
                    break;
                }
            }
        }

        //当添加数据时
        private void OnAddChoiceData(ChoiceData choiceData)
        {
            Port newPort = this.CreatePort(choiceData.Text);
            newPort.userData = choiceData;
            outputContainer.Add(newPort);
        }
        
        //当删除数据时
        private void OnDeleteChoiceData(ChoiceData choiceData)
        {
            Port portToRemove = null;

            foreach (Port port in outputContainer.Children())
            {
                if (port.userData == choiceData)
                {
                    portToRemove = port;
                    break;
                }
            }
            
            outputContainer.Remove(portToRemove);
            
        }
    }
}
