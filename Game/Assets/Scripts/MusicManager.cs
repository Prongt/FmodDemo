using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class MusicManager : MonoBehaviour
{
    [ParamRef] public string actionParam;
    public StudioEventEmitter eventEmitter;
    [EventRef] public string eventToPlay;
    [ParamRef] public string hatChangeParam;
    bool hatEquipped;
    bool state;
    
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
        if (Input.GetKeyDown(KeyCode.H)) EquipHat();

        if (Input.GetKeyDown(KeyCode.Space)) ChangeActionState();
    }

    void EquipHat()
    {
        hatEquipped = !hatEquipped;
        UiManager.DisplayHat = hatEquipped;
        RuntimeManager.StudioSystem.setParameterByName(hatChangeParam, hatEquipped ? 1.0f : 0.0f);
    }

    void ChangeActionState()
    {
        state = !state;
        RuntimeManager.StudioSystem.setParameterByName(actionParam, state ? 1.0f : 0.0f);
    }
}