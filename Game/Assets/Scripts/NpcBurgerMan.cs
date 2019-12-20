using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;


public class NpcBurgerMan : MonoBehaviour
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

        StartCoroutine(SpawnBurger());
        StopEmitters();
        OnBurgerEmitter.Play();
    }

    IEnumerator SpawnBurger()
    {
        yield return new WaitForSeconds(2.0f);
        Burger.SetActive(true);
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
        
        MusicManager.SetVolume(0.75f);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        BurgerUI.SetActive(false);
        playerInRange = false;
        
        MusicManager.SetVolume(1.0f);
    }
}