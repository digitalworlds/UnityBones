using UnityEngine;
using UnityEngine.UI;

public class SliderOrbitCamera : MonoBehaviour
{
    [Tooltip("The target object to orbit around.")]
    public Transform target;

    [Header("UI Sliders (normalized values 0 to 1)")]
    public Slider horizontalSlider;  // Controls horizontal rotation.
    public Slider verticalSlider;    // Controls vertical rotation.
    public Slider zoomSlider;        // Controls zoom.

    [Header("Rotation Settings")]
    [Tooltip("Total horizontal rotation range (in degrees).")]
    public float horizontalRotationRange = 360f;  // Maps to -180° to +180°.
    [Tooltip("Minimum vertical angle (in degrees).")]
    public float minVerticalAngle = -30f;
    [Tooltip("Maximum vertical angle (in degrees).")]
    public float maxVerticalAngle = 30f;

    [Header("Zoom Settings")]
    [Tooltip("Closest distance (min zoom).")]
    public float minZoom = 2f;
    [Tooltip("Farthest distance (max zoom).")]
    public float maxZoom = 50f;

    void Start()
    {
        // Set the sliders to 50% (if not already set).
        if (horizontalSlider != null) horizontalSlider.value = 0.5f;
        if (verticalSlider != null) verticalSlider.value = 0.5f;
        if (zoomSlider != null) zoomSlider.value = 0.5f;

        UpdateCamera();
    }

    void Update()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        if (target == null)
            return;

        // Read normalized slider values.
        float hValue = horizontalSlider.value;
        float vValue = verticalSlider.value;
        float zoomValue = zoomSlider.value;

        // Map horizontal slider: 0 => -180°, 1 => +180°; 0.5 yields 0°.
        float horizontalAngle = Mathf.Lerp(-horizontalRotationRange / 2f, horizontalRotationRange / 2f, hValue);
        // Map vertical slider: 0 => minVerticalAngle, 1 => maxVerticalAngle; 0.5 yields the midpoint.
        float verticalAngle = Mathf.Lerp(minVerticalAngle, maxVerticalAngle, vValue);

        // Build the rotation from the computed angles.
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);

        // Map zoom slider to a distance between minZoom and maxZoom.
        float distance = Mathf.Lerp(minZoom, maxZoom, zoomValue);

        // Calculate the camera position relative to the target.
        Vector3 negDistance = new Vector3(0f, 0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        // Update the camera's transform.
        transform.position = position;
        transform.rotation = rotation;
    }
}
