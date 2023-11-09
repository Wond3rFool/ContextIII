using UnityEngine;

public class ObjectSnap : MonoBehaviour
{
    public float snapDistance = 1.0f; // Adjust the distance for snapping

    private bool isDragging;

    private void Start()
    {
        isDragging = false;
    }

    private void Update()
    {
        if(isDragging)
        { 
            TrySnapObjects();
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

    private void TrySnapObjects()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (clickedObject.CompareTag("SnapObject"))
            {
                SnapToNearestObject(clickedObject);
            }
        }
    }

    private void SnapToNearestObject(GameObject targetObject)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, snapDistance);

        float closestDistance = float.MaxValue;
        GameObject closestObject = null;

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("SnapObject") && collider.gameObject != targetObject)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = collider.gameObject;
                }
            }
        }

        if (closestObject != null && closestDistance <= snapDistance)
        {
            // Snap the object to the closest object's position
            transform.position = closestObject.transform.position;
        }
    }
}