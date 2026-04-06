using Editor.Story;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BaseGroup : Group
{
    public string ID { get; set; }
    
    public string OldTitle { get; set; }

    public BaseGroup(string title, Vector2 position)
    {
        ID = UnityEngine.GUID.Generate().ToString();
        this.title = title;
        OldTitle = title;
        SetPosition(new Rect(position, Vector2.zero));
        
        headerContainer.AddToClassList("group__header-container");
    }

    public GroupData GetGroupData()
    {
        GroupData groupData = new GroupData()
        {
            GUID = ID,
            Title = title,
            Position = GetPosition().position
        };

        return groupData;
    }
}