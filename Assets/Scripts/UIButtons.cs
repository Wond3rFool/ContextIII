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
    private void Start()
    {
        handleButtons = GetComponentInChildren<HandleButton>();
    }

    private void Update()
    {
        if (handleButtons.GetLastSelectedGameObject() != null) 
        {
            objectToScale = handleButtons.GetLastSelectedGameObject();
        }
    }
    public void RotateCameraClockWise()
    {
        rotator.transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    public void RotateCamereCounterClockWise() 
    {
        rotator.transform.Rotate(0, -speed * Time.deltaTime, 0);
    }



}
