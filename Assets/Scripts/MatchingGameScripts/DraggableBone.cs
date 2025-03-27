// SamiyyahRucker(MadeLove)
// Makes bones moveable and matchable

using UnityEngine;

public class DraggableBone : MonoBehaviour
{
    public string boneId; // match this bone to its correct target

    private Vector3 correctPosition; // where the bone should snap to
    private Quaternion correctRotation; // roatation when snapped
    private bool isPlaced = false; // locks bone if correctly matched
    private bool isBeingDragged = false; // tracks if bone is currently being dragged

    void Start()
    {
        // ask manager for the correct position for reference bone
        Transform reference = MatchingGameManager.Instance.GetReferenceTransform(boneId);
        if (reference != null)
        {
            correctPosition = reference.position;
            correctRotation = reference.rotation;
        }
        else
        {
            Debug.LogWarning($"Reference not found for bone: {boneId}");
        }
    }

    void Update()
    {
        
        if (isBeingDragged && Input.GetMouseButtonUp(0)) // drag and realease 
        {
            isBeingDragged = false; // stops drag state

            float dist = Vector3.Distance(transform.position, correctPosition);
            if (dist < 0.5f)
            {
                transform.position = correctPosition;
                transform.rotation = correctRotation;
                isPlaced = true; // snaps in place locks and cant move again

                if (GetComponent<Rigidbody>() != null)
                    Destroy(GetComponent<Rigidbody>());

                PiecesPlacedTracker.instance.PiecesPlacedCounter();
                Debug.Log($"✅ {boneId} placed correctly.");
            }
        }
    }

    void OnMouseDown() // activates drag when pressed and bone isn’t placed
    {
        if (!isPlaced)
        {
            isBeingDragged = true;
        }
    }

    void OnMouseDrag() // mouse movements
    {
        if (!isPlaced)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
