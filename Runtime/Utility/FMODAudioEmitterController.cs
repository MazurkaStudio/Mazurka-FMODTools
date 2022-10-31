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
        [SerializeField] protected FMODAudioEmitter emitter;
        [SerializeField] protected EventReference eventRef;

        [SerializeField] protected MonobehaviourEvents startOn = MonobehaviourEvents.Custom;
        [SerializeField] protected MonobehaviourEvents stopOn = MonobehaviourEvents.Custom;
        [SerializeField] protected MonobehaviourEvents pauseOn = MonobehaviourEvents.Custom;
        [Tooltip("If instance not exist, resume will start the event")]
        [SerializeField] protected MonobehaviourEvents resumeOn = MonobehaviourEvents.Custom;
        [SerializeField] protected MonobehaviourEvents setParameterOn = MonobehaviourEvents.Custom;
        [SerializeField] protected MonobehaviourEvents stopAllOn = MonobehaviourEvents.Custom;
        [SerializeField] protected MonobehaviourEvents pauseAllOn = MonobehaviourEvents.Custom;
        [SerializeField] protected MonobehaviourEvents resumeAllOn = MonobehaviourEvents.Custom;



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
                emitter = GetComponent<FMODAudioEmitter>();
            }
        } 

        public void StartEvent()
        {
            if(playWithParameters)
            {
                if (isOnShot) emitter.PlayOneShot(eventRef, parameters);
                else eventInstance = emitter.PlaySound(eventRef, parameters);
            }
            else
            {
                if (isOnShot) emitter.PlayOneShot(eventRef);
                else eventInstance = emitter.PlaySound(eventRef);
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
            emitter.StopAllEventInstance(stopAllMode == FMOD.Studio.STOP_MODE.ALLOWFADEOUT ? true : false);
        }

        public void PauseAllEvent()
        {
            emitter.PauseAllEventInstance(true);
        }

        public void ResumeAllEvent()
        {
            emitter.PauseAllEventInstance(false);
        }


        [Sirenix.OdinInspector.Button("Stop All Emitters")]
        public void Test()
        {
            FMODAudioEmitterManager.StopAllEmittters();
        }

        protected virtual void Start()
        {
            if(startOn == MonobehaviourEvents.OnStart) StartEvent();
            if(stopOn == MonobehaviourEvents.OnStart) StopEvent();
            if(pauseOn == MonobehaviourEvents.OnStart) PauseEvent();
            if(resumeOn == MonobehaviourEvents.OnStart) ResumeEvent();
            if(setParameterOn == MonobehaviourEvents.OnStart) SetParameter();

            if(stopAllOn == MonobehaviourEvents.OnStart) StopAllEvent();
            if(pauseAllOn == MonobehaviourEvents.OnStart) PauseAllEvent();
            if(resumeAllOn == MonobehaviourEvents.OnStart) ResumeAllEvent();
        }

        protected virtual void OnEnable()
        {
            if (startOn == MonobehaviourEvents.OnEnable) StartEvent();
            if (stopOn == MonobehaviourEvents.OnEnable) StopEvent();
            if (pauseOn == MonobehaviourEvents.OnEnable) PauseEvent();
            if (resumeOn == MonobehaviourEvents.OnEnable) ResumeEvent();
            if (setParameterOn == MonobehaviourEvents.OnEnable) SetParameter(); 
            
            if (stopAllOn == MonobehaviourEvents.OnEnable) StopAllEvent();
            if (pauseAllOn == MonobehaviourEvents.OnEnable) PauseAllEvent();
            if (resumeAllOn == MonobehaviourEvents.OnEnable) ResumeAllEvent();
        }

        protected virtual void OnDisable()
        {
            if (startOn == MonobehaviourEvents.OnDisable) StartEvent();
            if (stopOn == MonobehaviourEvents.OnDisable) StopEvent();
            if (pauseOn == MonobehaviourEvents.OnDisable) PauseEvent();
            if (resumeOn == MonobehaviourEvents.OnDisable) ResumeEvent();
            if (setParameterOn == MonobehaviourEvents.OnDisable) SetParameter(); 
            
            if (stopAllOn == MonobehaviourEvents.OnDisable) StopAllEvent();
            if (pauseAllOn == MonobehaviourEvents.OnDisable) PauseAllEvent();
            if (resumeAllOn == MonobehaviourEvents.OnDisable) ResumeAllEvent();
        }

        protected virtual void OnDestroy()
        {

        }
    }
}
