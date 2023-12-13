using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool isDragging;

    private float rotationSpeed = 90.0f;

    private Renderer objectRenderer;

    private Material objectMaterial1;

    private HandleButton handleButton;

    private void Start()
    {
        isDragging = false;
        cam = FindAnyObjectByType<Camera>();
        handleButton = FindAnyObjectByType<HandleButton>();
        objectRenderer = GetComponentInChildren<Renderer>();
    }


    private void Update()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0; // Make sure the rotation is only in the horizontal plane
        forward.Normalize();
        string[] values = handleButton.GetValue();

        if (isDragging)
        {
            // Needed to move the object around with mouse position.
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7));
        }


    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
