using UnityEngine;

public class ReferenceSkullRotator : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private float rotationY = 0f;
    private Vector3 previousMousePosition;

    void Update()
    {
        if (DraggableBone.anyBoneBeingDragged) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.GetAxis("Mouse X");
            rotationY += deltaX * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x;
                rotationY += deltaX * rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
            }
        }
#endif
    }
}
