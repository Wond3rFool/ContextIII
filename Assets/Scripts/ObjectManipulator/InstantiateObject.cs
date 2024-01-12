using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToInstantiate1; // Reference to the prefab you want to instantiate
    [SerializeField]
    private GameObject prefabToInstantiate2; // Reference to the prefab you want to instantiate

    private Vector3 mousePosition;

    private void Update()
    {
        mousePosition = Input.mousePosition;

        // Convert the mouse position to a world point
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        if (Input.GetKeyDown(KeyCode.Alpha1)) Instantiate(prefabToInstantiate1, spawnPosition, Quaternion.identity);
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) Instantiate(prefabToInstantiate2, spawnPosition, Quaternion.identity);
        
    }
}