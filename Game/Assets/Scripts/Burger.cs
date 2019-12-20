using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class Burger : MonoBehaviour
{
    StudioEventEmitter eventEmitter;
    Animator animator;

    void Awake()
    {
        eventEmitter = eventEmitter ? eventEmitter : GetComponent<StudioEventEmitter>();
        animator = animator ? animator : GetComponent<Animator>();
        animator.enabled = false;
    }

    void OnEnable()
    {
        //StartCoroutine(MusicManager.ReduceVolume(7.0f, 0.5f));
        eventEmitter.Play();
        animator.enabled = true;
    }

    
}
