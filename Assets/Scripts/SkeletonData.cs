// Data structures to match JSON
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkeletonIndex
{
    public string last_updated;
    public List<SkeletonEntry> available_skeletons;
}

[System.Serializable]
public class SkeletonEntry
{
    public string id;
    public string species;
    public string data_file;
    public string glb_path;
}

[System.Serializable]
public class SkeletonData
{
    public Skeleton skeleton;
    public List<Bone> bones;
}

[System.Serializable]
public class Skeleton
{
    public string id;
    public string species;
    public string description;
    public string[] boneIds;
    public string[] skeletonFacts;
}

[System.Serializable]
public class Bone
{
    public string id;
    public string name;
    public string[] boneFacts;
}