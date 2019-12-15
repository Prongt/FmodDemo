using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class MusicManager : MonoBehaviour
{
    [EventRef] public string eventToPlay;
    [ParamRef] public string hatChangeParam;
    [ParamRef] public string actionParam;
    
    StudioEventEmitter eventEmitter;
    bool hatEquipped = false;
    bool state = false;


    void Awake()
    {
        eventEmitter = eventEmitter ? eventEmitter : GetComponent<StudioEventEmitter>();

        eventEmitter.Event = eventToPlay;
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
    }

    void ChangeState()
    {
        state = !state;
        Debug.Log(state);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(actionParam, state ? 1.0f : 0.0f);
    }
}