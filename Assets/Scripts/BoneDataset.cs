using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoneInfo
{
    public string id;   // "jaw"
    public string name; // "Jaw"
    public string fact; // "fact about jaw"
}

public class IdentifyingQuestions
{
    public string question;
    public string answerId; //id
}

public class MultipleChoiceQuestions
{
    public string question; 
    public string[] options;
    public int answer; 
}

public class Questions
{ 
    public List<IdentifyingQuestions> identifyingQuestions;
    public List<MultipleChoiceQuestions> multiplechoicequestions;
}

[System.Serializable]
public class BoneDataset
{
    public List<BoneInfo> bones;
    public List<Questions> questions;
    public string url;
}