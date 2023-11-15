using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToInstantiate1; // Reference to the prefab you want to instantiate
    [SerializeField]
    private GameObject prefabToInstantiate2; // Reference to the prefab you want to instantiate

    private Vector3 mousePos;

    private void Update()
    {
        mousePos = Input.mousePosition;
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 15f));

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Change 'Space' to the desired key
        {
            // Instantiate the prefab at a specific position and rotation
            Instantiate(prefabToInstantiate1, spawnPosition, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Change 'Space' to the desired key
        {
            // Instantiate the prefab at a specific position and rotation
            Instantiate(prefabToInstantiate2, spawnPosition, Quaternion.identity);
        }
    }
}