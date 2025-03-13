using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneScatterManager : MonoBehaviour
{
    public Transform scatterArea; // Right-side scatter zone
    public Transform referenceArea; // Left-side reference skull
    private Dictionary<string, Vector3> correctPositions = new Dictionary<string, Vector3>(); // Stores correct placements

    private List<GameObject> bones = new List<GameObject>();

    void Start()
    {
        StartCoroutine(WaitForGLTFLoad());
    }

    IEnumerator WaitForGLTFLoad()
    {
        Debug.Log("⏳ Waiting for .GLB to load...");

        yield return new WaitUntil(() => GameObject.Find("GLTFModel") != null);

        GameObject skullModel = GameObject.Find("GLTFModel");
        if (skullModel == null)
        {
            Debug.LogError("❌ ERROR: GLTF model not found after loading!");
            yield break;
        }

        Debug.Log("✅ .GLB Loaded! Separating bones...");
        SeparateAndScatterBones(skullModel);
    }

    void SeparateAndScatterBones(GameObject skullModel)
    {
        foreach (Transform child in skullModel.transform)
        {
            if (child.name.ToLower().Contains("skull"))
            {
                // Keep full skull as transparent reference
                GameObject transparentSkull = Instantiate(child.gameObject, referenceArea);
                MakeTransparent(transparentSkull);
            }
            else
            {
                // Scatter individual bones
                GameObject bone = Instantiate(child.gameObject, scatterArea);
                Vector3 correctPosition = child.position;
                correctPositions[bone.name] = correctPosition;

                bone.transform.position = new Vector3(
                    Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0
                );
                bone.AddComponent<DraggableBone>(); // Enable dragging
                bones.Add(bone);
            }
        }
        Debug.Log("✅ Bones separated & scattered.");
    }

    void MakeTransparent(GameObject obj)
    {
        foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
        {
            Material transparentMaterial = new Material(renderer.material);
            transparentMaterial.color = new Color(1, 1, 1, 0.3f); // 30% opacity
            renderer.material = transparentMaterial;
        }
    }

    public Vector3 GetCorrectPosition(string boneName)
    {
        return correctPositions.ContainsKey(boneName) ? correctPositions[boneName] : Vector3.zero;
    }
}
