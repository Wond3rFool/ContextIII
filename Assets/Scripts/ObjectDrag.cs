using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool isDragging;

    private float rotationSpeed = 90.0f;

    private void Start()
    {
        isDragging = false;
        cam = FindAnyObjectByType<Camera>();
    }


    private void Update()
    {
        if(isDragging)
        {
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7));
            if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.right * -rotationSpeed * Time.deltaTime);


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
