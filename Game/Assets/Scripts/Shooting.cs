using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public StudioEventEmitter eventEmitter;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            eventEmitter.Play();
        }
    }
}
