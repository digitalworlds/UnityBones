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

[System.Serializable]
public class BoneDataset
{
    public List<BoneInfo> bones;
    public string url;
}