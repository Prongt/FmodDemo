using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class MusicManager : MonoBehaviour
{
    [ParamRef] public string actionParam;
    public StudioEventEmitter eventEmitter;
    [EventRef] public string eventToPlay;
    [ParamRef] public string hatChangeParam;
    bool _hatEquipped;
    bool _state;
    
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

        //if (Input.GetKeyDown(KeyCode.Space)) ChangeActionState();
    }

    void EquipHat()
    {
        _hatEquipped = !_hatEquipped;
        UiManager.DisplayHat = _hatEquipped;
        RuntimeManager.StudioSystem.setParameterByName(hatChangeParam, _hatEquipped ? 1.0f : 0.0f);
    }

    public void ChangeActionState()
    {
        _state = !_state;
        RuntimeManager.StudioSystem.setParameterByName(actionParam, _state ? 1.0f : 0.0f);
    }
}