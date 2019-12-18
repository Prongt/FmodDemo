using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class MusicManager : MonoBehaviour
{
    public StudioEventEmitter eventEmitter;
    [EventRef] public string eventToPlay;
    [ParamRef] public string hatChangeParam;
    bool hatEquipped;
    [ParamRef] public string musicStateParam;

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
    }

    void EquipHat()
    {
        hatEquipped = !hatEquipped;
        UiManager.DisplayHat = hatEquipped;
        
        
        RuntimeManager.StudioSystem.setParameterByName(hatChangeParam, hatEquipped ? 1.0f : 0.0f);
    }

    public void ChangeMusicState(MusicState musicState)
    {
        RuntimeManager.StudioSystem.setParameterByName(musicStateParam, (int) musicState);
    }
}

public enum MusicState
{
    Core = 0,
    Omninus = 1,
    BurgerTime = 2
}