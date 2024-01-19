using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Quests/SpinningSymbol")]
public class SpinningSymbol : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 1);
    }
}

