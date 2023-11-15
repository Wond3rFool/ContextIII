using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool isDragging;

    private float rotationSpeed = 90.0f;

    private Renderer objectRenderer;

    private Material objectMaterial1;
    private Material objectMaterial2;
    private Material objectMaterial3;
    private Material objectMaterial4;

    private void Start()
    {
        isDragging = false;
        cam = FindAnyObjectByType<Camera>();
        objectRenderer = GetComponent<Renderer>();
    }


    private void Update()
    {
        if(isDragging)
        {
            //check where the mouse is for object dragging around
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15));

            //Rotation for the objects
            if (Input.GetKey(KeyCode.A)) transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D)) transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.W)) transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S)) transform.Rotate(Vector3.right * -rotationSpeed * Time.deltaTime);
            
            //changing the material for the object.
            if (Input.GetKey(KeyCode.Q)) objectRenderer.material = objectMaterial1;
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
