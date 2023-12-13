using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool isDragging;
    private Vector3 offset;

    private void Start()
    {
        isDragging = false;
        cam = FindAnyObjectByType<Camera>();
    }

    private void Update()
    {
        if (isDragging)
        {
            // Calculate the new position based on the offset
            Vector3 newPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7)) + offset;

            // Set the new position
            transform.position = newPosition;
        }
    }

    private void OnMouseDown()
    {
        // Calculate the offset between the object's position and the mouse position
        offset = transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7));

        // Start dragging
        isDragging = true;
    }

    private void OnMouseUp()
    {
        // Stop dragging
        isDragging = false;
    }

}
