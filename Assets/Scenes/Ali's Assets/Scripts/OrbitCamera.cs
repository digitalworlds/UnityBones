using UnityEngine;

public class ConsistentOrbitCamera : MonoBehaviour
{
    [Tooltip("The target object to orbit around.")]
    public Transform target;

    [Tooltip("Current distance from the target.")]
    public float distance = 10.0f;

    // Base speeds defined at the reference distance.
    [Tooltip("Orbit speed in degrees per second (constant regardless of zoom).")]
    public float baseOrbitSpeed = 180.0f;

    [Tooltip("Base pan speed at the reference distance.")]
    public float basePanSpeed = 1.0f;

    [Tooltip("Zoom speed (remains constant).")]
    public float baseZoomSpeed = 2.0f;

    [Tooltip("Reference distance at which the base pan speed is defined.")]
    public float referenceDistance = 10.0f;

    // Speeds used in the script.
    private float orbitSpeed;
    private float panSpeed;
    private float zoomSpeed;

    // Internal state for orbit angles.
    private float x = 0.0f;
    private float y = 0.0f;

    //check if toggle on or off
    private bool isToggleOn = false;

    // For panning calculations.
    private Vector3 lastMousePosition;

    void Start()
    {
        if (target != null)
        {
            // Initialize orbit angles from the current rotation.
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
        }
        CalibrateSpeeds();
    }

    void LateUpdate()
    {
        if (target == null || isToggleOn)
            return;

        // Calibrate panning speed each frame based on current distance.
        CalibrateSpeeds();

        // --- Orbiting (Left Mouse Button) ---
        // Here, we use a constant orbit speed so that the angular change per unit mouse movement remains fixed.
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * orbitSpeed * Time.deltaTime;
        }
        y = Mathf.Clamp(y, -80, 80);  // Optional clamp to avoid flipping.

        // Compute rotation and update camera position.
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;
        transform.rotation = rotation;
        transform.position = position;

        // --- Panning (Right Mouse Button) ---
        if (Input.GetMouseButtonDown(1))
            lastMousePosition = Input.mousePosition;
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            // Panning speed is scaled so that the translation feels consistent in screen space.
            Vector3 pan = transform.right * -delta.x * panSpeed * Time.deltaTime +
                          transform.up * -delta.y * panSpeed * Time.deltaTime;
            target.position += pan;
            transform.position += pan;
            lastMousePosition = Input.mousePosition;
        }

        // --- Zooming (Scroll Wheel) ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            distance -= scroll * baseZoomSpeed;
            distance = Mathf.Clamp(distance, 2.0f, 10.0f);
        }
    }

    void CalibrateSpeeds()
    {
        // For orbiting, we want a constant angular sensitivity.
        orbitSpeed = baseOrbitSpeed;
        // For panning, we scale based on current distance relative to our reference distance.
        panSpeed = basePanSpeed * (distance / referenceDistance);
        zoomSpeed = baseZoomSpeed; // remains unchanged.
    }
    public void ToggleTurnOn()
    {
        if (!isToggleOn) { isToggleOn = true; }
        else { isToggleOn = false; }
    }

}
