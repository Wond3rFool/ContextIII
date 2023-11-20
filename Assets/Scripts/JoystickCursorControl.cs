using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class JoystickCursorControl : MonoBehaviour
{
    public float cursorSpeed = 5.0f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Use the ArduinoValueReader script to get joystick input
        //float joystickX = float.Parse(ArduinoValueReader.Instance.value);

        // Adjust the cursor movement based on joystick input
        //moveDirection.x += joystickX;

        // Move the cursor
        transform.Translate(moveDirection * cursorSpeed * Time.deltaTime);
    }
}
