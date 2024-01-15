using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject rotator;

    [SerializeField]
    private float rotationSpeed = 25.0f;

    private HandleButton handleButtons;

    private GameObject objectToScale;

    private Quaternion targetRotation;
    private Vector3 targetScale;

    private float scaleFactorUp = 1.02f;
    private float scaleFactorDown = 0.98f;

    [SerializeField]
    private float scaleDuration = 0.3f;

    public AudioSource increaseSize;
    public AudioSource decreaseSize;

    public AudioSource rotateCamera;

    public AudioSource confirmButton;

    private bool rotateClockwise;
    private bool rotateCounterClockwise;
    private bool scaleUp;
    private bool scaleDown;

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

        if (rotateClockwise)
        {
            targetRotation *= Quaternion.Euler(0f, Mathf.Clamp(rotationSpeed, -180f, 180f) * Time.deltaTime, 0f);
        }

        if (rotateCounterClockwise)
        {
            targetRotation *= Quaternion.Euler(0f, Mathf.Clamp(-rotationSpeed, -180f, 180f) * Time.deltaTime, 0f);
        }

        if (scaleUp && objectToScale != null)
        {
            targetScale *= Mathf.Clamp(scaleFactorUp, 0.1f, 1.5f);
        }

        if (scaleDown && objectToScale != null)
        {
            targetScale *= Mathf.Clamp(scaleFactorDown, 0.1f, 1.5f);
        }

        if (objectToScale != null) objectToScale.transform.localScale = Vector3.Lerp(objectToScale.transform.localScale, targetScale, scaleDuration);
        rotator.transform.rotation = Quaternion.Lerp(rotator.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void RotateCameraClockwise()
    {
        rotateClockwise = true;
        rotateCamera.Play();
    }

    public void StopRotateCameraClockwise()
    {
        rotateClockwise = false;
        rotateCamera.Stop();
    }

    public void RotateCameraCounterClockwise()
    {
        rotateCounterClockwise = true;
        rotateCamera.Play();
    }

    public void StopRotateCameraCounterClockwise()
    {
        rotateCounterClockwise = false;
        rotateCamera.Stop();
    }

    public void ScaleObjectUp()
    {
        scaleUp = true;
        increaseSize.Play();
    }

    public void StopScaleObjectUp()
    {
        scaleUp = false;
        increaseSize.Stop();
    }

    public void ScaleObjectDown()
    {
        scaleDown = true;
        decreaseSize.Play();
    }

    public void StopScaleObjectDown()
    {
        scaleDown = false;
        decreaseSize.Stop();
    }
}
