using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool isDragging;

    private float rotationSpeed = 90.0f;

    private Renderer objectRenderer;

    private Material objectMaterial1;

    private void Start()
    {
        isDragging = false;
        cam = FindAnyObjectByType<Camera>();
        objectRenderer = GetComponentInChildren<Renderer>();
    }


    private void Update()
    {
        if(isDragging)
        {
            // Needed to move the object around with mouse position.
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7));

            // Rotate the object with WASD.
            if (Input.GetKey(KeyCode.A)) transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D)) transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.W)) transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S)) transform.Rotate(Vector3.right * -rotationSpeed * Time.deltaTime);

            // Change the material to something else.
            if (Input.GetKey(KeyCode.S)) objectRenderer.material = objectMaterial1;
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
