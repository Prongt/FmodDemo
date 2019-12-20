using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

[RequireComponent(typeof(StudioEventEmitter))]
public class FootStepAudioController : MonoBehaviour
{
    public StudioEventEmitter footStepEmitter;
    [EventRef]public string footStepEvent;
    [ParamRef]public string floorType;

    float floorValue;
    void Awake()
    {
        //footStepEmitter = GetComponent<StudioEventEmitter>();
        footStepEmitter.Event = footStepEvent;
    }

    void Update()
    {
        CheckFloorType();
    }

    public void PlayFootStep()
    {
        footStepEmitter.Play();
    }

    public void PlayJumpSound()
    {
        
    }
    

    void CheckFloorType()
    {
        var ray = new Ray(transform.position, Vector3.down);
        
        if (!Physics.Raycast(ray, out var hitInfo, 5.0f)) return;
        
        if (hitInfo.collider.CompareTag("Floor/Wood"))
        {
            floorValue = 0.0f;
        }
            
        if (hitInfo.collider.CompareTag("Floor/Concrete"))
        {
            floorValue = 2.0f;
        }
        
        RuntimeManager.StudioSystem.setParameterByName(floorType, floorValue);
    }
}