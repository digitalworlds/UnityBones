// SamiyyahRucker(MadeLove)
// Makes bones moveable and matchable

using System.Runtime.CompilerServices;
using UnityEngine;

public class DraggableBone : MonoBehaviour
{
    public static DraggableBone currentlySelectedBone = null;
    public static bool anyBoneBeingDragged = false;

    public string boneId; // match this bone to its correct target

    private bool isPlaced = false;
    private bool isBeingDragged = false;
    private bool isHovered = false;

    private Transform reference;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialScreenX;

    void Start()
    {
        reference = MatchingGameManager.Instance.GetReferenceTransform(boneId);
        if (reference == null)
        {
            Debug.LogWarning($"Reference not found for bone: {boneId}");
        }
    }

    void Update()
    {
        // Handle mouse release
        if (isBeingDragged && Input.GetMouseButtonUp(0))
        {
            if (reference != null)
            {
                Renderer renderer = reference.GetComponentInChildren<Renderer>();
                if (renderer != null)
                    renderer.material.SetColor("_Color", new Color(0, 0, 1, 0.1f));
            }

            isBeingDragged = false;
            anyBoneBeingDragged = false;
            isHovered = false;

            float dist = Vector3.Distance(transform.position, reference.position);
            Debug.Log(dist);

            if (dist < 0.5f)
            {
                transform.position = reference.position;
                transform.rotation = reference.rotation;
                isPlaced = true;
                reference.gameObject.SetActive(false);
                transform.SetParent(reference.parent);

                if (GetComponent<Rigidbody>() != null)
                    Destroy(GetComponent<Rigidbody>());

                BoneInfoUIManager.Instance.HideInfo();
                PiecesPlacedTracker.instance.PiecesPlacedCounter();
                Debug.Log($"✅ {boneId} placed correctly.");
            }
        }

        // Hide panel if user clicks away and this bone is the selected one
        if (Input.GetMouseButtonDown(0) && !isBeingDragged && !isPlaced && currentlySelectedBone == this)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject != this.gameObject)
                {
                    isHovered = false;
                    BoneInfoUIManager.Instance.HideInfo();
                }
            }
            else
            {
                // Clicked empty space
                isHovered = false;
                BoneInfoUIManager.Instance.HideInfo();
            }
        }
    }

    void OnMouseDown()
    {
        // Highlight reference bone
        if (reference != null)
        {
            Renderer renderer = reference.GetComponentInChildren<Renderer>();
            if (renderer != null)
                renderer.material.SetColor("_Color", new Color(1, 0, 0, 0.2f));
        }

        if (!isPlaced)
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialScreenX = Camera.main.WorldToScreenPoint(transform.position).x;
            isBeingDragged = true;
            anyBoneBeingDragged = true;
            isHovered = true;

            currentlySelectedBone = this; // ✅ Mark this bone as selected

            // Show bone info from JSON
            BoneInfo info = BoneLoader.Instance.GetBoneInfo(boneId);
            Debug.Log($"🦴 Clicked bone: {boneId}");
            if (info != null)
            {
                Debug.Log($"✅ Bone info found: {info.name} - {info.fact}");
                BoneInfoUIManager.Instance.ShowInfo(info.name, info.fact);
            }
            else
            {
                Debug.LogWarning($"❗ Bone info not found in JSON for ID: {boneId}");
            }
        }
    }

    void OnMouseDrag()
    {
        if (!isPlaced)
        {
            Vector3 refp = Camera.main.WorldToScreenPoint(reference.position);
            refp.y = 0; refp.z = 0;
            Vector3 p = Camera.main.WorldToScreenPoint(transform.position);
            p.y = 0; p.z = 0;

            float dist = Vector3.Distance(refp, new Vector3(initialScreenX, 0, 0));
            Vector3 delta = p - refp;

            float w = dist != 0 ? delta.x / dist : 0f;
            w = Mathf.Clamp01(w);

            transform.position = Vector3.Lerp(reference.position, initialPosition, w);
            transform.rotation = Quaternion.Lerp(reference.rotation, initialRotation, w);

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    public bool IsDragging()
    {
        return isBeingDragged;
    }
}
