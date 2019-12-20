using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ChangeParameters : MonoBehaviour
{
    public bool onEnter;
    public bool onExit;
    public List<Param> OnEnterList = new List<Param>();
    public List<Param> OnExitList = new List<Param>();

    void OnTriggerEnter(Collider other)
    {
        if (!onEnter) return;
        foreach (var param in OnEnterList) RuntimeManager.StudioSystem.setParameterByName(param.param, param.value);
    }

    void OnTriggerExit(Collider other)
    {
        if (!onExit) return;
        foreach (var param in OnExitList) RuntimeManager.StudioSystem.setParameterByName(param.param, param.value);
    }

    [Serializable]
    public struct Param
    {
        [ParamRef] public string param;
        public float value;
    }
}