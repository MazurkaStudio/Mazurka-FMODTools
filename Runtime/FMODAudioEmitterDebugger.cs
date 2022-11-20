using FMOD.Studio;
using FMODUnity;
using MazurkaGameKit.FMODTools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAudioEmitterDebugger : MonoBehaviour
{
    public FMODAudioEmitter targetEmitter;
    public EventInstance eventInstance;
    public EventReference eventRef;

    public int instanceCount;

    [Button("Play")]
    public void Play()
    {
        if (targetEmitter.PlaySound(eventRef, out EventInstance eventInstance))
            this.eventInstance = eventInstance;

        RetreiveInstanceCount();
    }

    [Button("Stop")]
    public void Stop()
    {
        targetEmitter.StopSound(eventInstance);
        RetreiveInstanceCount();
    }

    [Button("StopAll")]
    public void StopAll()
    {
        targetEmitter.StopEmitter();
        RetreiveInstanceCount();
    }

    [Button("Refresh")]
    public void Refresh()
    {
        targetEmitter.RefreshEventsInstances();
        RetreiveInstanceCount();
    }

    [Button("Pause")]
    public void Pause()
    {
        targetEmitter.PauseSound(eventInstance, !targetEmitter.IsInPause);
        RetreiveInstanceCount();
    }

    [Button("PauseAll")]
    public void PauseAll()
    {
        targetEmitter.PauseEmitter(!targetEmitter.IsInPause);
        RetreiveInstanceCount();
    }

    public void RetreiveInstanceCount()
    {
        switch (targetEmitter)
        {
            case FMODComplexAudioEmitter complex:
                instanceCount = complex.GetEventInstances.Count;
                break;

            case FMODSimpleAudioEmitter simple:
                instanceCount = simple.GetEventInstance.isValid() ? 1 : 0;
                break;
        }
    }
}
