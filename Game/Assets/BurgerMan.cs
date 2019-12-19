using System;
using FMODUnity;
using UnityEngine;


public class BurgerMan : MonoBehaviour
{
    public GameObject Burger;
    public GameObject BurgerUI;
    [EventRef] public string OnEnter;
    [EventRef] public string OnRequestsBurger;
    

    bool playerInRange;
    StudioEventEmitter OnEnterEmitter;
    StudioEventEmitter OnBurgerEmitter;

    void Awake()
    {
        // OnEnterEmitter = OnEnterEmitter ? OnEnterEmitter : GetComponent<StudioEventEmitter>();
        // OnBurgerEmitter = OnBurgerEmitter ? OnBurgerEmitter : GetComponent<StudioEventEmitter>();

        OnEnterEmitter = gameObject.AddComponent<StudioEventEmitter>();
        OnBurgerEmitter = gameObject.AddComponent<StudioEventEmitter>();

        OnEnterEmitter.Event = OnEnter;
        OnBurgerEmitter.Event = OnRequestsBurger;
    }

    void Update()
    {
        if (!playerInRange) return;

        if (!Input.GetKeyDown(KeyCode.E)) return;
        
        Burger.SetActive(true);
            
        StopEmitters();
        OnBurgerEmitter.Play();
    }

    void StopEmitters()
    {
        OnBurgerEmitter.Stop();
        OnEnterEmitter.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        BurgerUI.SetActive(true);
        playerInRange = true;
        
        StopEmitters();
        OnEnterEmitter.Play();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        BurgerUI.SetActive(false);
        playerInRange = false;
    }
}