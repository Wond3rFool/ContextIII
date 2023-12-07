using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BareBonesDrag : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool isDragging;

    private HandleButton handleButton;

    private void Start()
    {
        isDragging = false;
        cam = FindAnyObjectByType<Camera>();
    }


    private void Update()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0; // Make sure the rotation is only in the horizontal plane
        forward.Normalize();

        if (isDragging)
        {
            // Needed to move the object around with mouse position.
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15));

            // Rotate the object with WASD.

            // Change the material to something else.
            //if (Input.GetKey(KeyCode.Q)) objectRenderer.material = objectMaterial1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x - 10, Input.mousePosition.y));
        if (Input.GetKey(KeyCode.RightArrow)) Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x + 10, Input.mousePosition.y));
        if (Input.GetKey(KeyCode.Space)) MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);


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
