// SamiyyahRucker(MadeLove)
// Makes bones moveable and matchable

using System.Runtime.CompilerServices;
using UnityEngine;

public class DraggableBone : MonoBehaviour
{
    public string boneId; // match this bone to its correct target

    private bool isPlaced = false; // locks bone if correctly matched
    private bool isBeingDragged = false; // tracks if bone is currently being dragged

    private Transform reference; 

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialScreenX;

    void Start()
    {
        // ask manager for the correct position for reference bone
        reference = MatchingGameManager.Instance.GetReferenceTransform(boneId);
        if (reference == null)
        {
            Debug.LogWarning($"Reference not found for bone: {boneId}");
        }
    }

    void Update()
    {
        
        if (isBeingDragged && Input.GetMouseButtonUp(0)) // drag and realease 
        {
            GameObject obj=reference.gameObject;
            Renderer renderer = obj.GetComponentInChildren<Renderer>();
            renderer.material.SetColor("_Color", new Color(0, 0, 1, 0.1f));

            isBeingDragged = false; // stops drag state

            float dist = Vector3.Distance(transform.position, reference.position);
            Debug.Log(dist);

            if (dist < 0.5f)
            {
                transform.position = reference.position;
                transform.rotation = reference.rotation;
                isPlaced = true; // snaps in place locks and cant move again
                reference.gameObject.SetActive(false);

                if (GetComponent<Rigidbody>() != null)
                    Destroy(GetComponent<Rigidbody>());

                PiecesPlacedTracker.instance.PiecesPlacedCounter();
                Debug.Log($"✅ {boneId} placed correctly.");
            }
        }
    }

    void OnMouseDown() // activates drag when pressed and bone isn’t placed
    {
        GameObject obj=reference.gameObject;
        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        renderer.material.SetColor("_Color", new Color(1, 0, 0, 0.2f));

        if (!isPlaced)
        {
            initialPosition=transform.position;
            initialRotation=transform.rotation;
            initialScreenX=Camera.main.WorldToScreenPoint(transform.position).x;
            isBeingDragged = true;
            
        }
    }

    void OnMouseDrag() // mouse movements
    {
        if (!isPlaced)
        {
            
            Vector3 refp=Camera.main.WorldToScreenPoint(reference.position);
            refp.y=0;refp.z=0;
            Vector3 p=Camera.main.WorldToScreenPoint(transform.position);
            p.y=0;p.z=0;
            float dist=Vector3.Distance(refp,new Vector3(initialScreenX,0,0));
            Vector3 delta=p-refp;

            float w=delta.x/dist;
            Debug.Log(this.name+" "+dist);
            if(w<0)w=0;else if(w>1)w=1;
            transform.position=Vector3.Lerp(reference.position,initialPosition,w);
            transform.rotation=Quaternion.Lerp(reference.rotation,initialRotation,w);

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
