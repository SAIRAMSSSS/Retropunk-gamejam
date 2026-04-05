using System;
using UnityEngine;

[Serializable]
public class DialogueData
{
    public string speaker;
    public string text;
    public string animation;
    public int animationLayer;
}

[Serializable]
public class Dialogue
{
    public string dialogueName;
    public bool phoneCall;
    public DialogueData[] lines;
}

public class DialogueParser
{
    public Dialogue LoadDialogue(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogues/{fileName}");
        if( jsonFile != null)
        {
            Dialogue dialogue = JsonUtility.FromJson<Dialogue>(jsonFile.text);
            return dialogue;
        }
        return null;
    }
}
