using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class BurgerManager : MonoBehaviour
{
    public GameObject Burger;
    public GameObject BurgerUI;
    [EventRef] public string OnEnter;
    [EventRef] public string OnRequestsBurger;
    

    bool playerInRange;
    StudioEventEmitter eventEmitter;

    void Awake()
    {
        eventEmitter = eventEmitter ? eventEmitter : GetComponent<StudioEventEmitter>();
    }
}
