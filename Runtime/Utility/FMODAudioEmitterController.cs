using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{

    //TODO: rework, c'est pas ouf la maniere de trigger des controles


    public class FMODAudioEmitterController : MonoBehaviour
    {
        [SerializeField] protected FMODComplexAudioEmitter emitter;
        [SerializeField] protected EventReference eventRef;

        [SerializeField] protected StartSoundEvent startOn = StartSoundEvent.Custom;
        [SerializeField] protected StartSoundEvent stopOn = StartSoundEvent.Custom;
        [SerializeField] protected StartSoundEvent pauseOn = StartSoundEvent.Custom;
        [Tooltip("If instance not exist, resume will start the event")]
        [SerializeField] protected StartSoundEvent resumeOn = StartSoundEvent.Custom;
        [SerializeField] protected StartSoundEvent setParameterOn = StartSoundEvent.Custom;
        [SerializeField] protected StartSoundEvent stopAllOn = StartSoundEvent.Custom;
        [SerializeField] protected StartSoundEvent pauseAllOn = StartSoundEvent.Custom;
        [SerializeField] protected StartSoundEvent resumeAllOn = StartSoundEvent.Custom;



        [SerializeField] protected FMOD.Studio.STOP_MODE stopMode;
        [SerializeField] protected FMOD.Studio.STOP_MODE stopAllMode;

        protected EventInstance eventInstance;

        [SerializeField] protected ParamRef[] parameters;
        [Tooltip("One shot is short and non-loopable sound")]
        [SerializeField] protected bool isOnShot = false;
        [SerializeField] protected bool playWithParameters = false;

        protected virtual void OnValidate()
        {
            if(emitter == null)
            {
                emitter = GetComponent<FMODComplexAudioEmitter>();
            }
        } 

        public void StartEvent()
        {
            if(playWithParameters)
            {
                if (isOnShot) emitter.PlayOneShot(eventRef, parameters);
                else if (emitter.PlaySound(eventRef, parameters, out EventInstance eventInstance))
                    this.eventInstance = eventInstance;
            }
            else
            {
                if (isOnShot) emitter.PlayOneShot(eventRef);
                else if (emitter.PlaySound(eventRef, out EventInstance eventInstance))
                    this.eventInstance = eventInstance;
            }
        }

        public void StopEvent()
        {
            emitter.StopSound(eventInstance, stopMode);
        }

        public void PauseEvent()
        {
            emitter.PauseSound(eventInstance, true);
        }

        public void ResumeEvent()
        {
            if (eventInstance.isValid())
                emitter.PauseSound(eventInstance, false);
            else StartEvent();
        }

        public void SetParameter()
        {
            emitter.SetParameter(eventInstance, parameters);
        }

        public void StopAllEvent()
        {
            emitter.StopEmitter(stopAllMode == FMOD.Studio.STOP_MODE.ALLOWFADEOUT ? true : false);
        }

        public void PauseAllEvent()
        {
            emitter.PauseEmitter(true);
        }

        public void ResumeAllEvent()
        {
            emitter.PauseEmitter(false);
        }


        [Sirenix.OdinInspector.Button("Stop All Emitters")]
        public void Test()
        {
            FMODAudioEmitterManager.StopAllEmittters();
        }

        protected virtual void Start()
        {
            if(startOn == StartSoundEvent.OnStart) StartEvent();
            if(stopOn == StartSoundEvent.OnStart) StopEvent();
            if(pauseOn == StartSoundEvent.OnStart) PauseEvent();
            if(resumeOn == StartSoundEvent.OnStart) ResumeEvent();
            if(setParameterOn == StartSoundEvent.OnStart) SetParameter();

            if(stopAllOn == StartSoundEvent.OnStart) StopAllEvent();
            if(pauseAllOn == StartSoundEvent.OnStart) PauseAllEvent();
            if(resumeAllOn == StartSoundEvent.OnStart) ResumeAllEvent();
        }

        protected virtual void OnEnable()
        {
            if (startOn == StartSoundEvent.OnEnable) StartEvent();
            if (stopOn == StartSoundEvent.OnEnable) StopEvent();
            if (pauseOn == StartSoundEvent.OnEnable) PauseEvent();
            if (resumeOn == StartSoundEvent.OnEnable) ResumeEvent();
            if (setParameterOn == StartSoundEvent.OnEnable) SetParameter(); 
            
            if (stopAllOn == StartSoundEvent.OnEnable) StopAllEvent();
            if (pauseAllOn == StartSoundEvent.OnEnable) PauseAllEvent();
            if (resumeAllOn == StartSoundEvent.OnEnable) ResumeAllEvent();
        }

        protected virtual void OnDisable()
        {
            if (startOn == StartSoundEvent.OnDisable) StartEvent();
            if (stopOn == StartSoundEvent.OnDisable) StopEvent();
            if (pauseOn == StartSoundEvent.OnDisable) PauseEvent();
            if (resumeOn == StartSoundEvent.OnDisable) ResumeEvent();
            if (setParameterOn == StartSoundEvent.OnDisable) SetParameter(); 
            
            if (stopAllOn == StartSoundEvent.OnDisable) StopAllEvent();
            if (pauseAllOn == StartSoundEvent.OnDisable) PauseAllEvent();
            if (resumeAllOn == StartSoundEvent.OnDisable) ResumeAllEvent();
        }

        protected virtual void OnDestroy()
        {

        }
    }
}
