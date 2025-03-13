using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBone : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private bool isPlacedCorrectly = false;
    private Transform parentAfterDrag;
    private BoneScatterManager scatterManager;

    void Start()
    {
        originalPosition = transform.position;

        // Find the BoneScatterManager in the scene
        scatterManager = FindObjectOfType<BoneScatterManager>();

        if (scatterManager == null)
        {
            Debug.LogError("❌ ERROR: BoneScatterManager not found in scene!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlacedCorrectly) return;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); // Bring bone to front while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlacedCorrectly) return;

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        newPosition.z = 0;
        transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scatterManager == null) return;

        if (CheckIfCorrect())
        {
            isPlacedCorrectly = true;
            transform.position = scatterManager.GetCorrectPosition(gameObject.name); // Snap to correct position
            transform.SetParent(parentAfterDrag); // Lock in place
            Debug.Log("✅ Correct placement: " + gameObject.name);
        }
        else
        {
            transform.position = originalPosition; // Reset position if incorrect
            Debug.Log("❌ Wrong placement: " + gameObject.name);
        }
    }

    private bool CheckIfCorrect()
    {
        if (scatterManager == null) return false;

        Vector3 referencePosition = scatterManager.GetCorrectPosition(gameObject.name);
        return Vector3.Distance(transform.position, referencePosition) < 0.5f;
    }
}
