using UnityEngine;
using UnityEngine.EventSystems;

public class BoneInteraction : MonoBehaviour
{
    private Bone boneData;
    private SkeletonLoader skeletonLoader;

    public void Initialize(Bone bone, SkeletonLoader loader)
    {
        boneData = bone;
        skeletonLoader = loader;
    }

    public Bone GetBoneData() => boneData;
}