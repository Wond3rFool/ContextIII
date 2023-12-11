using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashcan : MonoBehaviour
{
    bool isDeleting;
    float timer;
    ObjectDrag draggableObject1;
    BareBonesDrag draggableObject2;
    private void Awake()
    {
        isDeleting = false;
        timer = 0;
    }
    private void Update()
    {
        Debug.Log(timer);
        if (isDeleting)
        {
            timer += Time.deltaTime;
            if (timer > 3)
            {
                if (draggableObject1 != null) 
                {
                    Destroy(draggableObject1.gameObject);
                    isDeleting = false;
                    timer = 0;
                }
                if (draggableObject2 != null) 
                {
                    Destroy(draggableObject2.gameObject);
                    isDeleting = false;
                    timer = 0;
                }
                    
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        draggableObject1 = other.GetComponent<ObjectDrag>();
        draggableObject2 = other.GetComponent<BareBonesDrag>();

        if (draggableObject1 != null || draggableObject2 != null)
        {
            isDeleting = true;
            Debug.Log("hovering");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        draggableObject1 = other.GetComponent<ObjectDrag>();
        draggableObject2 = other.GetComponent<BareBonesDrag>();

        if (draggableObject1 != null || draggableObject2 != null)
        {
            isDeleting = false;
            timer = 0;
            Debug.Log("left");
        }
    }
}

