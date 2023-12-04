using UnityEngine;

public class PersistObject : MonoBehaviour
{
    private void Start()
    {
        // Make the object persist between scenes
        DontDestroyOnLoad(gameObject);
    }
}