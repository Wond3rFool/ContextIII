using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject rotator;

    [SerializeField]
    private float speed;

    private HandleButton handleButtons;

    private GameObject objectToScale;

    private Quaternion targetRotation;
    private Vector3 targetScale;

    private float scaleFactorUp = 1.1f;
    private float scaleFactorDown = 0.9f;// You can adjust this factor based on your requirements
    private float scaleDuration = 10f; // Duration for each scaling step

    private float rotationSpeed = 10.0f; // You can adjust the rotation speed based on your requirements
    private float rotationDuration = 4.0f; // Duration for each rotation step


    private void Start()
    {
        handleButtons = GetComponentInChildren<HandleButton>();
        targetRotation = rotator.transform.rotation;
    }

    private void Update()
    {
        if (handleButtons.GetLastSelectedGameObject() != null) 
        {
            objectToScale = handleButtons.GetLastSelectedGameObject();
            targetScale = objectToScale.transform.localScale;
            
        }
        print(targetScale);
        if (targetScale != Vector3.zero) 
        {
            objectToScale.transform.localScale = Vector3.Lerp(objectToScale.transform.localScale, targetScale, scaleDuration * Time.deltaTime);
        }
        rotator.transform.rotation = Quaternion.Lerp(rotator.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    public void RotateCameraClockWise()
    {
        targetRotation *= Quaternion.Euler(0f, 20f, 0f);
    }

    public void RotateCamereCounterClockWise() 
    {
        targetRotation *= Quaternion.Euler(0f, -20f, 0f);
    }

    public void ScaleObjectUp()
    {
        if (objectToScale == null) return;
        targetScale *= scaleFactorUp;
    }

    public void ScaleObjectDown()
    {
        if (objectToScale == null) return;
        targetScale *= scaleFactorDown;
    }
}
