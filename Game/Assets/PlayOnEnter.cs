using System;
using UnityEngine;

public class PlayOnEnter : MonoBehaviour
{
    public MusicState musicStateOnEnter;
    public MusicState musicStateOnExit;

    MusicManager musicManager;

    void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) musicManager.ChangeMusicState(musicStateOnEnter);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) musicManager.ChangeMusicState(musicStateOnExit);
    }
}