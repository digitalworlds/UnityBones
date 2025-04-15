using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;
    public GameObject object5;

    private GameObject activeObject; // Stores the currently visible object

    void Start()
    {
        // Set the default active object to null (nothing visible initially)
        activeObject = null;
       // activeObject = object1;
        ToggleObject(object5);
        ToggleObject(object4);
        ToggleObject(object3);
        ToggleObject(object2);
        ToggleObject(object1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleObject(object1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleObject(object2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleObject(object3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleObject(object4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ToggleObject(object5);
        }
    }

    void ToggleObject(GameObject obj)
    {
        if (activeObject == obj)
        {
            obj.SetActive(false);
            activeObject = null;
        }
        else
        {
            if (activeObject != null)
                activeObject.SetActive(false); // Hide the previous object

            obj.SetActive(true);
            activeObject = obj;
        }
    }
}
