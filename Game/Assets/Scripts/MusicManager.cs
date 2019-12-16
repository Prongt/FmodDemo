using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class MusicManager : MonoBehaviour
{
    [EventRef] public string eventToPlay;
    [ParamRef] public string hatChangeParam;
    [ParamRef] public string actionParam;
    [ParamRef] public string distortParam;
    
    public StudioEventEmitter eventEmitter;
    bool hatEquipped = false;
    bool state = false;
    bool distort = false;


    void Awake()
    {
        FindComponents();
        eventEmitter.Event = eventToPlay;
    }

    void FindComponents()
    {
        eventEmitter = eventEmitter ? eventEmitter : GetComponent<StudioEventEmitter>();
    }

    void OnValidate()
    {
        FindComponents();
    }

    void Start()
    {
        eventEmitter.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        { 
            hatEquipped = !hatEquipped;
            //Debug.Log("Hat changed");
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(hatChangeParam,  hatEquipped ? 1.0f : 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeDistort();
        }
    }

    void ChangeState()
    {
        state = !state;
        //Debug.Log(state );
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(actionParam, state ? 1.0f : 0.0f);
    }
    
    void ChangeDistort()
    {
        distort = !distort;
        //Debug.Log(distort);

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(distortParam, distort ? 1.0f : 0.0f);
    }
}