using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class BurgerManager : MonoBehaviour
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
        eventEmitter.Play();
        animator.enabled = true;
    }
}
