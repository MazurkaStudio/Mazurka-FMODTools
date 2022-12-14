using FMOD.Studio;
using FMODUnity;
using MazurkaGameKit.FMODTools;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAudioEmitterDebugger : MonoBehaviour
{
    public FMODAudioEmitter targetEmitter;
    public EventInstance eventInstance;
    public EventReference eventRef;

    public int instanceCount;
    
#if ODIN_INSPECTOR
    [Button("Play")]
#endif
    public void Play()
    {
        if (targetEmitter.PlaySound(eventRef, out EventInstance eventInstance))
            this.eventInstance = eventInstance;

        RetreiveInstanceCount();
    }
    
#if ODIN_INSPECTOR
    [Button("Stop")]
#endif
    public void Stop()
    {
        targetEmitter.StopSound(eventInstance);
        RetreiveInstanceCount();
    }
    
#if ODIN_INSPECTOR
    [Button("StopAll")]
#endif
    public void StopAll()
    {
        targetEmitter.StopEmitter();
        RetreiveInstanceCount();
    }
    
#if ODIN_INSPECTOR
    [Button("Refresh")]
#endif
    public void Refresh()
    {
        targetEmitter.RefreshEventsInstances();
        RetreiveInstanceCount();
    }
    
#if ODIN_INSPECTOR
    [Button("Pause")]
#endif
    public void Pause()
    {
        targetEmitter.PauseSound(eventInstance, !targetEmitter.IsInPause);
        RetreiveInstanceCount();
    }
    
#if ODIN_INSPECTOR
    [Button("PauseAll")]
#endif
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
