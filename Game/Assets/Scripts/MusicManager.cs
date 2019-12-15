using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class MusicManager : MonoBehaviour
{
    [EventRef] public string fmodEvent;
    StudioEventEmitter eventEmitter;
    bool hatEquipped = false;


    void Awake()
    {
        eventEmitter = eventEmitter ? eventEmitter : GetComponent<StudioEventEmitter>();

        eventEmitter.Event = fmodEvent;
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
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("HatEquipped",  hatEquipped ? 1.0f : 0.0f);
        }
    }
}