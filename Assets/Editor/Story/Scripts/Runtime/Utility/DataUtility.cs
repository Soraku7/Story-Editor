using System.Collections.Generic;
using Editor.Story;

public static class DataUtility
{
    public static List<ChoiceData> CloneChoiceChoices(List<ChoiceData> oldDatas)
    {
        List<ChoiceData> newDatas = new List<ChoiceData>();
        if (oldDatas == null) return newDatas;
        foreach (ChoiceData data in oldDatas)
        {
            ChoiceData newData = new ChoiceData(data.Text, data.NextNodeID);
            newDatas.Add(newData);
        }

        return newDatas;
    }

    public static List<SentenceData> CloneSenteenceDatas(List<SentenceData> oldDatas)
    {
        if (oldDatas == null) return null;

        List<SentenceData> newDatas = new List<SentenceData>();
        foreach (SentenceData data in oldDatas)
        {
            SentenceData newData = new SentenceData(data.Text);
            newDatas.Add(newData);
        }

        return newDatas;
    }
}