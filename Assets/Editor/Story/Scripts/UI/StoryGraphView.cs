using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public class StoryGraphView : GraphView
    {
        //关联窗口
        private StoryEditorWindow storyEditorWindow;
        private NodeCreationBox nodeCreationBox;

        public List<BaseGroup> Groups
        {
            get
            {
                List<BaseGroup> groups = new List<BaseGroup>();
                graphElements.ForEach(element =>
                {
                    if (element is BaseGroup group)
                    {
                        groups.Add(group);
                    }
                });
                return groups;
            }
        }

        public List<BaseNode> Nodes
        {
            get
            {
                List<BaseNode> nodes = new List<BaseNode>();
                graphElements.ForEach(element =>
                {
                    if (element is BaseNode node)
                    {
                        nodes.Add(node);
                    }
                });
                return nodes;
            }
        }

        public StoryGraphView(StoryEditorWindow window)
        {
            //实例化时绑定窗口
            storyEditorWindow = window;

            AddGridBackground();
            AddManipulators();
            AddDefaultNode();
            AddNodeCreationBox();

            OnOpenNodeCreationBox();
            OnGraphViewChange();
            OnElementsReadyDelete();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRename();
            OnCopyElement();
            OnPasteElement();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            //添加右键菜单
            evt.menu.AppendAction("添加节点", (action) =>
            {
                //获取光标位置
                Vector2 screenMousePosition = action.eventInfo.mousePosition + new Vector2(50, 35);
                //出发请求事件
                nodeCreationRequest(new NodeCreationContext()
                {
                    screenMousePosition = screenMousePosition,
                    index = -1
                });
            });

            evt.menu.AppendAction("添加分组", (action) => { CreateGroup("分组", GetLocalMousePosition(action.eventInfo.localMousePosition)); });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> result = ports.ToList();

            result = result.Where(endport => endport.direction != startPort.direction && endport.node != startPort.node).ToList();

            return result;
        }

        //添加网格背景
        private void AddGridBackground()
        {
            //创建网格背景
            GridBackground gridBackground = new();
            //设置网格背景拉伸与视图相同
            gridBackground.StretchToParentSize();
            //添加到GraphView
            Insert(0, gridBackground);
        }

        //添加试图操作
        private void AddManipulators()
        {
            //添加视图缩放
            // this.AddManipulators(new ContentZoomer());
            //滚轮缩放
            SetupZoom(0.2f, 2.0f);
            // SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //graphview窗口内容的拖动
            this.AddManipulator(new ContentDragger());
            //选中Node移动功能
            this.AddManipulator(new SelectionDragger());
            //多个node框选功能
            this.AddManipulator(new RectangleSelector());
        }

        //创建节点
        public BaseNode CreateNode(string title, NodeType type, Vector2 position, Group group = null, bool shouldDraw = true)
        {
            //获取节点类型
            Type nodeType = Type.GetType("Editor.Story." + type + "Node");

            Debug.Log(type);
            //创建节点
            BaseNode node = Activator.CreateInstance(nodeType) as BaseNode;
            //初始化节点
            node.Init(this, title, position);

            if (shouldDraw)
            {
                node.Draw();
            }

            if (group == null)
            {
                AddElement(node);
            }
            else
            {
                group.AddElement(node);
            }

            return node;
        }

        public Edge CreateEdge(Port lastOutput, Port nextInput)
        {
            Edge edge = lastOutput.ConnectTo(nextInput);
            AddElement(edge);
            return edge;
        }

        public BaseGroup CreateGroup(string title, Vector2 position, bool moveSelectedNodes = true)
        {
            BaseGroup group = new(title, position);
            AddElement(group);

            if (moveSelectedNodes)
            {
                //如果选中了多个节点然后创建分组，则将这些节点放入新的分组
                foreach (GraphElement item in selection)
                {
                    if (item is BaseNode baseNode)
                    {
                        group.AddElement(baseNode);
                    }
                }
            }

            return group;
        }

        //添加默认节点
        public void AddDefaultNode()
        {
            CreateNode("开始", NodeType.Start, Vector2.zero);
            CreateNode("结束", NodeType.End, new Vector2(500, 0));
        }

        //添加节点创建框
        private void AddNodeCreationBox()
        {
            nodeCreationBox = ScriptableObject.CreateInstance<NodeCreationBox>();
            nodeCreationBox.Init(this);
        }

        //打开添加节点对话框
        private void OnOpenNodeCreationBox()
        {
            //定义请求事件
            nodeCreationRequest = context =>
            {
                //打开节点创建框
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), nodeCreationBox);
            };
        }

        private void OnGraphViewChange()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        OnCreateEdge(edge);
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    GraphElement startNodes = changes.elementsToRemove.FirstOrDefault(e => e is StartNode);
                    changes.elementsToRemove.Remove(startNodes);
                    List<GraphElement> endNodes = changes.elementsToRemove.Where(e => e is EndNode).ToList();

                    if (endNodes.Count == GetEndNodesAmount())
                    {
                        GraphElement lastEndNode = endNodes.Last();
                        changes.elementsToRemove.Remove(lastEndNode);
                    }

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element is BaseNode node)
                        {
                            OnDeleteNode(node);
                        }

                        else if (element is BaseGroup group)
                        {
                            OnDeleteGroup(group);
                        }
                        else if (element is Edge edge)
                        {
                            OnDeleteEdge(edge);
                        }
                    }
                }

                if (changes.movedElements != null)
                {
                }

                return changes;
            };
        }

        private void OnElementsReadyDelete()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<ISelectable> readyToDelete = new List<ISelectable>();
                foreach (GraphElement element in selection)
                {
                    if (element is BaseNode node)
                    {
                        if (node is StartNode)
                        {
                            string str = "不可删除开始节点";
                            EditorUtility.DisplayDialog("警告", str, "确定");
                            continue;
                        }

                        if (node is EndNode)
                        {
                            if (GetEndNodesAmount() == 1)
                            {
                                string str = "至少要有一个结束节点";
                                EditorUtility.DisplayDialog("警告", str, "确定");
                                continue;
                            }
                        }

                        readyToDelete.Add(node);
                    }
                    else if (element is BaseGroup group)
                    {
                        readyToDelete.Add(group);
                    }
                    else if (element is Edge edge)
                    {
                        readyToDelete.Add(edge);
                    }
                }

                selection = readyToDelete;
                DeleteSelection();
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                BaseGroup baseGroup = (BaseGroup)group;
                foreach (GraphElement element in elements)
                {
                    if (element is BaseNode node)
                    {
                        node.Group = baseGroup;
                    }
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                BaseGroup baseGroup = (BaseGroup)group;
                foreach (GraphElement element in elements)
                {
                    if (element is BaseNode node)
                    {
                        node.Group = null;
                    }
                }
            };
        }

        private void OnGroupRename()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                BaseGroup baseGroup = (BaseGroup)group;

                string temp = newTitle;
                temp.RemoveSpecialCharacters();
                temp.RemoveWhitespace();
                baseGroup.title = temp;
            };
        }

        private void OnCopyElement()
        {
            serializeGraphElements = (elements) =>
            {
                CopyDatas copyDatas = new CopyDatas();

                foreach (var element in elements)
                {
                    if (element is BaseNode node)
                    {
                        if (node.Type == NodeType.Start)
                        {
                            continue;
                        }

                        NodeData nodeData = node.GetNodeData();
                        copyDatas.nodeDatas.Add(nodeData);
                    }
                    else if (element is BaseGroup group)
                    {
                        GroupData groupData = group.GetGroupData();
                        copyDatas.groupDatas.Add(groupData);
                    }
                }

                string temp = JsonUtility.ToJson(copyDatas, true);

                return temp;
            };
        }

        private void OnPasteElement()
        {
            unserializeAndPaste = (operationName, data) =>
            {
                ClearSelection();

                CopyDatas copyDatas = JsonUtility.FromJson<CopyDatas>(data);

                Dictionary<GroupData, BaseGroup> pasteGroups = new Dictionary<GroupData, BaseGroup>();
                Dictionary<NodeData, BaseNode> pasteNodes = new Dictionary<NodeData, BaseNode>();

                foreach (GroupData groupData in copyDatas.groupDatas)
                {
                    string newTitle = groupData.Title;
                    Vector2 newPosition = groupData.Position + new Vector2(50, 50);

                    BaseGroup group = CreateGroup(newTitle, newPosition, false);
                    pasteGroups.Add(groupData, group);
                }

                foreach (NodeData nodeData in copyDatas.nodeDatas)
                {
                    string newTitle = nodeData.Title;
                    Vector2 newPosition = nodeData.Position + new Vector2(50, 50);
                    BaseNode node = CreateNode(newTitle, nodeData.Type, newPosition, null, false);

                    pasteNodes.Add(nodeData, node);

                    node.Note = nodeData.Note;
                    node.ChoiceDatas = DataUtility.CloneChoiceChoices(nodeData.ChoiceDatas);

                    if (node.Type == NodeType.Dialogue)
                    {
                        DialogueNode dialogueNode = node as DialogueNode;
                        dialogueNode.RoleName = nodeData.RoleName;
                        dialogueNode.SentenceDatas = DataUtility.CloneSenteenceDatas(nodeData.sentenceDatas);
                    }

                    node.Draw();
                }

                foreach (var pasteNode in pasteNodes)
                {
                    NodeData nodeData = pasteNode.Key;
                    BaseNode node = pasteNode.Value;

                    //更新分组信息
                    if (!string.IsNullOrEmpty(nodeData.GroupID))
                    {
                        foreach (GroupData id in pasteGroups.Keys)
                        {
                            if (id.GUID == nodeData.GroupID)
                            {
                                pasteGroups[id].AddElement(node);
                                node.Title = nodeData.Title;
                                break;
                            }
                        }
                    }

                    foreach (Port outputPort in node.outputContainer.Children())
                    {
                        ChoiceData choiceData = (ChoiceData)outputPort.userData;
                        if (string.IsNullOrEmpty(choiceData.NextNodeID))
                        {
                            continue;
                        }

                        NodeData nextNodeData = pasteNodes.Keys.FirstOrDefault(x => x.GUID == choiceData.NextNodeID);

                        if (nextNodeData == null)
                        {
                            choiceData.NextNodeID = "";
                            continue;
                        }
                        
                        BaseNode nextNode = pasteNodes[nextNodeData];
                        Port nextnodeInputPort = (Port)nextNode.inputContainer.Children().First();
                        choiceData.NextNodeID = nextNode.GUID;
                        
                        CreateEdge(outputPort, nextnodeInputPort);
                    }
                    node.RefreshPorts();
                }
            };
        }

        private int GetEndNodesAmount()
        {
            return graphElements.Where(e => e is EndNode).ToList().Count();
        }

        private void OnCreateEdge(Edge edge)
        {
            BaseNode nextNode = (BaseNode)edge.input.node;
            ChoiceData choiceData = (ChoiceData)edge.output.userData;
            choiceData.NextNodeID = nextNode.GUID;
        }

        private void OnDeleteNode(BaseNode node)
        {
            Debug.Log("删除节点");
        }

        private void OnDeleteGroup(BaseGroup group)
        {
            Debug.Log("删除分组");
        }

        private void OnDeleteEdge(Edge edge)
        {
            if (edge.output == null) return;
            ChoiceData choiceData = (ChoiceData)edge.output.userData;
            choiceData.NextNodeID = "";
        }

        public Vector2 GetLocalMousePosition(Vector2 screenMousePosition, bool isNodeCreationBox = false)
        {
            Vector2 windowMousePosition;
            if (isNodeCreationBox)
            {
                //将光标的屏幕坐标转换为窗口内的坐标
                windowMousePosition = screenMousePosition - storyEditorWindow.position.position;
            }
            else
            {
                windowMousePosition = screenMousePosition;
            }

            //将光标在当前窗口内的坐标转换为节点视图内的坐标
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);

            return localMousePosition;
        }

        public void ClearGraph()
        {
            graphElements.ForEach(element => RemoveElement(element));
        }
    }
}