using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickCursorControl : MonoBehaviour
{
    public float cursorSpeed = 5.0f;

    private bool isDragging = false;

    void Update()
    {
        // Get joystick input for cursor movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Move the cursor based on joystick input
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
        transform.Translate(moveDirection * cursorSpeed * Time.deltaTime);

        // Check for joystick button press
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.isPressed)
            {
                isDragging = true;
            }
            else
            {
                isDragging = false;
            }
        }

        // Drag the game object when the joystick button is pressed
        if (isDragging)
        {
            // Add your code for dragging the game object here
            // For example: transform.Translate(moveDirection * cursorSpeed * Time.deltaTime);
        }

        // Check for additional keyboard input
        if (Input.GetKey(KeyCode.LeftArrow)) Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x - 10, Input.mousePosition.y));
        if (Input.GetKey(KeyCode.RightArrow)) Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x + 10, Input.mousePosition.y));
        if (Input.GetKey(KeyCode.KeypadEnter)) MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
    }
}
