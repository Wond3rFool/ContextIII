using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool isDragging;
    private bool canPlay;
    private Vector3 offset;
    private Vector3 objectZ;

    private void Start()
    {
        isDragging = false;
        canPlay = true;
        cam = FindAnyObjectByType<Camera>();
    }

    private void Update()
    {
        if (isDragging)
        {
            objectZ = WorldToScreen(transform.position);
            // Calculate the new position based on the offset
            Vector3 newPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, objectZ.z)) + offset;

            // Set the new position
            transform.position = newPosition;
        }
    }

    private void OnMouseDown()
    {
        if (UIButtons.usingUI) 
        {
            isDragging = false;
            canPlay = true;
            return;
        }
        objectZ = WorldToScreen(transform.position);
        // Calculate the offset between the object's position and the mouse position
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, objectZ.z));

        // Calculate the offset from the corner of the object
        offset = transform.position - worldMousePos;

        // Start dragging
        isDragging = true;

        if (canPlay) 
        {
            canPlay = false;
        }

        
    }

    private void OnMouseUp()
    {
        // Stop dragging
        isDragging = false;
        canPlay = true;
    }

    private Vector3 WorldToScreen(Vector3 worldPos)
    {
        return cam.WorldToScreenPoint(worldPos);
    }

}
