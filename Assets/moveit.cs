using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class moveit : MonoBehaviour
{
    public float Uno;
    public float Dos;
    public float Tres;

    public float ein;
    public float zwei;
    public float drei;


    void Update()
    {
        transform.Rotate(Uno * Time.deltaTime, Dos * Time.deltaTime, Tres * Time.deltaTime);
        transform.Translate(ein * Time.deltaTime, zwei * Time.deltaTime, drei * Time.deltaTime);
    }
}
