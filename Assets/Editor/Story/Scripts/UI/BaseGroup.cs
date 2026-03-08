using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BaseGroup : Group
{
    public string ID { get; set; }
    
    public string OldTitle { get; set; }

    public BaseGroup(string title, Vector2 position)
    {
        ID = UnityEditor.GUID.Generate().ToString();
        this.title = title;
        OldTitle = title;
        SetPosition(new Rect(position, Vector2.zero));
        
        headerContainer.AddToClassList("group__header-container");
    }
}