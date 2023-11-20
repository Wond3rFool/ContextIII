using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToInstantiate1; // Reference to the prefab you want to instantiate
    [SerializeField]
    private GameObject prefabToInstantiate2; // Reference to the prefab you want to instantiate

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Change 'Space' to the desired key
        {
            // Instantiate the prefab at a specific position and rotation
            Instantiate(prefabToInstantiate1, new Vector3(0, 0, 15), Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Change 'Space' to the desired key
        {
            // Instantiate the prefab at a specific position and rotation
            Instantiate(prefabToInstantiate2, new Vector3(0, 0, 15), Quaternion.identity);
        }
    }
}