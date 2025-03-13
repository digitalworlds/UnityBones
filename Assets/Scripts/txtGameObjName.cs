using UnityEngine;
using UnityEngine.UI;

public class txtGameObjName:MonoBehaviour
{
    public Text boneText;
    public GameObject bone;
    private void Start()
    {
        if (boneText != null)
            boneText.text = bone.ToString();
    }
}
