// SamiyyahRucker(MadeLove)
// Spawns puzzle pieces

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BoneScatterManager : MonoBehaviour
{
    // Add reference section here
    [Header("Reference Bone Settings")]
    public Transform referenceArea;

    // Add scatter section here
    [Header("Draggable Bone Settings")]
    public Transform scatterArea;
    public Vector3 scatterBounds = new Vector3(2f, 2f, 2f); // adjust as needed

    //Spawns reference and draggable bones
    void Start()
    {
        StartCoroutine(SpawnReferenceBones());
        StartCoroutine(SpawnScatteredBones());
    }

    private IEnumerator SpawnReferenceBones()
    {
        yield return new WaitUntil(() => BoneLoader.Instance.IsGLBLoaded()); // wait until loaded

        foreach (var entry in BoneLoader.Instance.GetBoneDictionary()) // loops through each bone in JSON file
        {
            string boneId = entry.Key;
            GameObject bone = BoneLoader.Instance.GetBoneObject(boneId); // copy each bone

            if (bone != null)
            {
                GameObject referenceBone = Instantiate(bone, referenceArea); // copy each bone in reference area
                referenceBone.name = boneId + "_Reference";
                SetTransparency(referenceBone, 0.5f);
                //MakeTransparent(referenceBone); // suppose to be see through - need to work on this 🔙

                MatchingGameManager.Instance.RegisterCorrectPosition(boneId, referenceBone.transform);// correct target position for that bome

            }
        }

        Debug.Log("✅ Reference bones created.");
    }

    private void SetTransparency(GameObject obj, float alpha)
    {
        Renderer renderer = obj.GetComponentInChildren<Renderer>();

        
        if (renderer != null)
        {
            renderer.material=null;
            Material newMaterial = new Material(Resources.Load<Material>("TransparentMaterial"));
            if (newMaterial != null)
            {
                renderer.material = newMaterial;
                renderer.material.SetColor("_Color", new Color(0, 0, 1, 0.1f));
            }
            else
            {
                Debug.LogWarning("TransparentMaterial not found in Resources!");
            }
        }
    }


    private IEnumerator SpawnScatteredBones()
    {
        yield return new WaitUntil(() => BoneLoader.Instance.IsGLBLoaded()); // wait again for glb to finish loading 

        foreach (var entry in BoneLoader.Instance.GetBoneDictionary()) // loop through bone database
        {
            string boneId = entry.Key;
            GameObject bonePrefab = BoneLoader.Instance.GetBoneObject(boneId);

            if (bonePrefab != null)
            {
                // Spanw bones for draggable
                GameObject draggableBone = Instantiate(bonePrefab);
                draggableBone.name = boneId;
                draggableBone.transform.parent = null;

            

                // Calculate random position inside scatter area
                Vector3 scatterCenter = scatterArea.position;
                Vector3 randomOffset = new Vector3(
                    Random.Range(-scatterBounds.x, scatterBounds.x),
                    Random.Range(-scatterBounds.y, scatterBounds.y),
                    Random.Range(-scatterBounds.z, scatterBounds.z)
                );
                Vector3 scatterPosition = scatterCenter + 0*randomOffset;

                draggableBone.transform.position = scatterPosition;
                draggableBone.transform.rotation = Quaternion.Euler( // randomly rotates the bone on all 3 axes 
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f)
                );

                // Drag and Drop bones by ID
                if (!draggableBone.TryGetComponent<DraggableBone>(out _))
                {
                    var drag = draggableBone.AddComponent<DraggableBone>();
                    drag.boneId = boneId;
                }

                MeshFilter childMeshFilter = draggableBone.GetComponentInChildren<MeshFilter>();
                // Add MeshCollider to trigger interaction
                if (draggableBone.GetComponent<Collider>() == null)
                {
                    // Create a new mesh instance (avoid modifying the shared asset)
                    Mesh transformedMesh = Instantiate(childMeshFilter.sharedMesh);

                    // Apply child's transform to the mesh vertices
                    Transform childTransform = childMeshFilter.transform;
                    Vector3[] vertices = transformedMesh.vertices;
                    
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i] = childTransform.TransformPoint(vertices[i]); // Convert to world space
                        vertices[i] = draggableBone.transform.InverseTransformPoint(vertices[i]); // Convert to parent space
                    }
                    
                    transformedMesh.vertices = vertices;
                    transformedMesh.RecalculateBounds();

                    // Add MeshCollider to parent and assign the transformed mesh
                    MeshCollider collider = draggableBone.AddComponent<MeshCollider>();
                    collider.sharedMesh = transformedMesh;

                    //Rigidbody rigidbody=draggableBone.AddComponent<Rigidbody>();
                    
                }

                //Debug.Log($"🦴 {boneId} scattered to {scatterPosition}");
            }
        }



        Debug.Log("✅ All bones scattered!");
    }

    // suppose to help with transparancy of reference skull but its not giving the look I desire - will come 🔙 
    private void MakeTransparent(GameObject bone)
    {
        foreach (Renderer r in bone.GetComponentsInChildren<Renderer>())
        {
            Material mat = new Material(r.material);
            Color color = mat.color;
            color.a = 0.3f;
            mat.color = color;

            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;

            r.material = mat;
        }
    }
}
