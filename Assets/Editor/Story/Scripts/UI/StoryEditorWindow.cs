using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Story
{
    public class StoryEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "StoryName";
        private readonly string keyLastStoryName = "currentStoryName";
        private readonly string variablePath = "Assets/Editor/Story/Style Sheets/Variables.uss";
        private readonly string toolbarStylePath = "Assets/Editor/Story/Style Sheets/ToolbarStyle.uss";
        private readonly string graphViewStylePath = "Assets/Editor/Story/Style Sheets/GraphViewStyle.uss";

        private readonly string storyFloderPath = "Assets/Editor/Story";
        private readonly string exampleFolderPath = "Assets/Editor/Story/Example";
        private readonly string exampleFolderName = "Example";
        private readonly string storyDatasFolderPath = "Assets/Editor/Story/StoryDatas";
        private readonly string storyDataFolerName = "StoryData";

        private string fileName;
        private StoryDataSO storyData;

        private StoryGraphView graphView;

        private Toolbar toolbar;
        private static TextField tfdFileName;
        private Button btnSave;
        private Button btnOpen;
        private Button btnNew;
        private Button btnClear;
        private Button btnMiniMap;


        [MenuItem("Tools/StoryEditorWindow %&S")] // Ctrl + Alt + S
        public static void ShowExample()
        {
            //获取窗口
            StoryEditorWindow wnd = GetWindow<StoryEditorWindow>();
            //窗口标题
            wnd.titleContent = new GUIContent("StoryEditorWindow");
        }

        public void CreateGUI()
        {
            AddToolbar();

            AddGraphView();

            AddStyles();
            OpenLastStory();
        }

        [OnOpenAsset()]
        public static bool OnDoubleClick(int instanceID)
        {
            StoryEditorWindow wnd = (StoryEditorWindow)GetWindow(typeof(StoryEditorWindow));
            if (wnd == null)
            {
                ShowExample();
            }

            wnd.RemoveNotification();

            string fullPath = AssetDatabase.GetAssetPath(instanceID);
            StoryDataSO storyData = IOUtility.LoadAsset<StoryDataSO>(fullPath);
            if (storyData == null)
            {
                return false;
            }

            string str = "确认打开新故事并覆盖当前视图内容？未保存数据将无法恢复";
            if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
            {
                wnd.storyData = storyData;
                wnd.RecordCurrentStory();

                wnd.graphView.ClearGraph();
                wnd.LoadDatas(storyData);

                string message = "故事已经打开";
                wnd.ShowNotification(new GUIContent(message));
            }

            return true;
        }

        //添加工具栏
        private void AddToolbar()
        {
            //创建UI元素
            tfdFileName = ElementUtility.CreateTextField(defaultFileName, "新故事", callback =>
            {
                if (callback.newValue.HasSpecialCharacter())
                {
                    string temp = callback.newValue.RemoveSpecialCharacters();
                    tfdFileName.value = temp;
                    fileName = temp;
                }
                else
                {
                    fileName = callback.newValue;
                }
            });
            btnSave = ElementUtility.CreateButton("保存", SaveStory);
            btnOpen = ElementUtility.CreateButton("打开", OpenStory);
            btnNew = ElementUtility.CreateButton("新建", NewStory);
            btnClear = ElementUtility.CreateButton("清空", ClearGraphAndCreateDefaultDatas);
            btnMiniMap = ElementUtility.CreateButton("小地图", null);

            //创建工具栏
            toolbar = new();

            //添加UI元素到工具栏
            toolbar.Add(tfdFileName);
            toolbar.Add(btnSave);
            toolbar.Add(btnOpen);
            toolbar.Add(btnNew);
            toolbar.Add(btnClear);
            toolbar.Add(btnMiniMap);

            //工具栏加入到窗口
            rootVisualElement.Add(toolbar);
            fileName = defaultFileName;
        }

        private void AddGraphView()
        {
            graphView = new StoryGraphView(this);

            //将尺寸拉至与窗口相同
            // graphView.StretchToParentSize();
            // //将视图放入窗口中
            // rootVisualElement.Insert(0, graphView);
            rootVisualElement.Add(graphView);
        }

        //添加样式文件
        private void AddStyles()
        {
            //引用变量样式文件
            rootVisualElement.AddStyleSheet(variablePath);
            toolbar.AddStyleSheet(toolbarStylePath);
            graphView.AddStyleSheet(graphViewStylePath);
        }

        private void SaveStory()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                string str = "故事名称不能为空";
                EditorUtility.DisplayDialog("警告", str, "确定");
                return;
            }

            IOUtility.CreateFolder(storyFloderPath, exampleFolderName);
            IOUtility.CreateFolder(exampleFolderPath, storyDataFolerName);

            storyData = IOUtility.CreateAsset<StoryDataSO>(storyDatasFolderPath, fileName);
            storyData.Init(fileName);

            SaveDatas();

            string message = "保存成功";
            ShowNotification(new GUIContent(message));
        }

        private void OpenStory()
        {
            string filePath = EditorUtility.OpenFilePanel("打开故事", storyDatasFolderPath, "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            //转相对路径
            filePath = FileUtil.GetProjectRelativePath(filePath);

            StoryDataSO story = IOUtility.LoadAsset<StoryDataSO>(filePath);

            if (story == null)
            {
                string temp = "故事不存在：\n\n" + $"{filePath}\n\n" + "请确保你选择了正确的文件";
                EditorUtility.DisplayDialog("警告", temp, "确定");
                return;
            }

            string str = "是否覆盖当前故事？";
            if (EditorUtility.DisplayDialog("警告", str, "确定", "取消"))
            {
                storyData = story;
                RecordCurrentStory();

                graphView.ClearGraph();
                LoadDatas(storyData);
                string message = "打开成功";
                ShowNotification(new GUIContent(message));
            }
        }

        private void NewStory()
        {
            string str = "是否新建故事?";
            if (EditorUtility.DisplayDialog("警告", str, "确定", "取消"))
            {
                graphView.ClearGraph();
                graphView.AddDefaultNode();
                UpdateFileName(defaultFileName);
                
                string message = "创建成功";
                ShowNotification(new GUIContent(message));
            }
        }

        private void ClearGraphAndCreateDefaultDatas()
        {
            string str = "是否清空当前故事？";
            if (EditorUtility.DisplayDialog("警告", str, "确定", "取消"))
            {
                graphView.ClearGraph();
                graphView.AddDefaultNode();
                
                string message = "清空成功";
                ShowNotification(new GUIContent(message));
            }
        }

        private void OpenLastStory()
        {
            string storyNmae = EditorPrefs.GetString(keyLastStoryName);

            if (string.IsNullOrEmpty(storyNmae))
            {
                return;
            }

            StoryDataSO story = IOUtility.LoadAsset<StoryDataSO>(storyDatasFolderPath, storyNmae);

            string message;
            if (story == null)
            {
                message = $"未找到上次编辑的故事";
                ShowNotification(new GUIContent(message));
                return;
            }

            storyData = story;
            graphView.ClearGraph();
            LoadDatas(storyData);

            message = $"已打开上次编辑的故事";
            ShowNotification(new GUIContent(message));
        }

        private void RecordCurrentStory()
        {
            EditorPrefs.SetString(keyLastStoryName , storyData.FileName);
        }

        private void SaveDatas()
        {
            SaveGroupDatas(graphView.Groups);
            SaveNodeDatas(graphView.Nodes);
            IOUtility.SaveAsset(storyData);
        }

        private void SaveNodeDatas(List<BaseNode> nodeDatas)
        {
            foreach (BaseNode group in nodeDatas)
            {
                NodeData nodeData = group.GetNodeData();
                storyData.NodeDatas.Add(nodeData);
            }
        }

        private void SaveGroupDatas(List<BaseGroup> groupDatas)
        {
            foreach (BaseGroup group in groupDatas)
            {
                GroupData groupData = group.GetGroupData();
                storyData.GroupDatas.Add(groupData);
            }
        }

        private void LoadDatas(StoryDataSO storyData)
        {
            UpdateFileName(storyData.FileName);
            Dictionary<string, BaseGroup> loadedGroups = LoadGroups(storyData.GroupDatas);
            Dictionary<string, BaseNode> loadedNodes = LoadNodes(storyData.NodeDatas, loadedGroups);
            LoadNodesConnections(loadedNodes);
        }

        private void LoadNodesConnections(Dictionary<string, BaseNode> loadedNodes)
        {
            foreach (var node in loadedNodes)
            {
                foreach (Port outputPort in node.Value.outputContainer.Children())
                {
                    ChoiceData choiceData = (ChoiceData)outputPort.userData;
                    if (string.IsNullOrEmpty(choiceData.NextNodeID))
                    {
                        continue;
                    }

                    Port nextnodeInputPort = loadedNodes[choiceData.NextNodeID].Input;
                    graphView.CreateEdge(outputPort, nextnodeInputPort);
                }

                node.Value.RefreshPorts();
            }
        }

        private Dictionary<string, BaseNode> LoadNodes(List<NodeData> storyDataNodeDatas, Dictionary<string, BaseGroup> loadedGroups)
        {
            Dictionary<string, BaseNode> loadedNodes = new Dictionary<string, BaseNode>();

            foreach (NodeData nodeData in storyDataNodeDatas)
            {
                BaseNode node = graphView.CreateNode(nodeData.Title, nodeData.Type, nodeData.Position, null, false);
                node.GUID = nodeData.GUID;
                node.Note = nodeData.Note;
                node.ChoiceDatas = DataUtility.CloneChoiceChoices(nodeData.ChoiceDatas);

                if (node.Type == NodeType.Dialogue)
                {
                    DialogueNode dialogueNode = node as DialogueNode;
                    if (dialogueNode != null)
                    {
                        dialogueNode.RoleName = nodeData.RoleName;
                        dialogueNode.SentenceDatas = DataUtility.CloneSenteenceDatas(nodeData.sentenceDatas);
                    }
                }

                node.Draw();

                BaseGroup group = null;
                if (!string.IsNullOrEmpty(nodeData.GroupID))
                {
                    group = loadedGroups[nodeData.GroupID];
                    group.AddElement(node);
                }

                loadedNodes.Add(node.GUID, node);
            }

            return loadedNodes;
        }

        private Dictionary<string, BaseGroup> LoadGroups(List<GroupData> storyDataGroupDatas)
        {
            Dictionary<string, BaseGroup> loadedGroups = new Dictionary<string, BaseGroup>();

            foreach (GroupData groupData in storyDataGroupDatas)
            {
                BaseGroup group = graphView.CreateGroup(groupData.Title, groupData.Position);
                group.ID = groupData.GUID;

                loadedGroups.Add(group.ID, group);
            }

            return loadedGroups;
        }

        private void UpdateFileName(string storyDataFileName)
        {
            tfdFileName.value = storyDataFileName;
        }
    }
}