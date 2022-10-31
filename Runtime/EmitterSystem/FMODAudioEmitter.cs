using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    /// <summary>
    /// Base class to use audio emitter in static emitter event system
    /// </summary>
    public class FMODAudioEmitter : MonoBehaviour, IFMODAudioEmitter
    {
        #region Interface

        #region Emitter Registration

        public virtual void RegisterEmitter() => FMODAudioEmitterManager.Register(this);

        public virtual void UnregisterEmitter() => FMODAudioEmitterManager.Unregister(this);

        #endregion

        #region Event Instances Registration

        public List<EventInstance> GetEventInstances => eventInstances;

        protected List<EventInstance> eventInstances = new List<EventInstance>();

        public virtual void RegisterNewEventInstance(EventInstance eventInstance)
        {
            if (!eventInstance.isValid())
                return;

            if (eventInstances.Contains(eventInstance))
                return;

            eventInstances.Add(eventInstance);
        }

        public virtual void UnregisterNewEventInstance(EventInstance eventInstance)
        {
            if (!eventInstances.Contains(eventInstance))
                return;

            eventInstances.Remove(eventInstance);
        }

        public void RefreshEventsInstances()
        {
            for (int i = eventInstances.Count - 1; i >= 0; i--)
            {
                if (!eventInstances[i].isValid())
                {
                    eventInstances.RemoveAt(i);
                }
            }
        }

        #endregion

        public Transform GetSoundEmitter => overrrideSoundSource == null ? transform : overrrideSoundSource;

        public bool IsInPause => isInPause;

        public bool CanEmitSound
        {
            get
            {
                return canEmitSound;
            }
            set
            {
                if(canEmitSound != value)
                {
                    if(!value)
                    {
                        StopAllEventInstance();
                    }
                }
            }
        }

        public virtual void StopAllEventInstance(bool allowFadeOut = true)
        {
            FMOD.Studio.STOP_MODE stopMode = allowFadeOut ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE;

            foreach (EventInstance instances in eventInstances)
            {
                StopSound(instances, stopMode);
            }

            eventInstances.Clear();
        }

        public virtual void PauseAllEventInstance(bool value)
        {
            if (canBePaused)
            {
                isInPause = value;

                RefreshEventsInstances();

                if (value)
                {
                    foreach (EventInstance instances in eventInstances)
                    {
                        OnSoundWillPaused(instances);
                        instances.setPaused(value);
                    }
                }
                else
                {
                    foreach (EventInstance instances in eventInstances)
                    {
                        instances.setPaused(value);
                        OnSoundWasResumed(instances);
                    }
                }
                   
            }
        }

        #endregion


        [SerializeField] private Transform overrrideSoundSource;
        [SerializeField] private bool canBePaused = true;

        protected bool isInPause = false;

        protected bool canEmitSound = true;

        #region Mono

        protected virtual void Start()
        {
            RegisterEmitter();
        }

        protected virtual void OnDestroy()
        {
            StopAllEventInstance();
            UnregisterEmitter();
        }

        #endregion

        #region Utility

        public virtual void PlayOneShot(EventReference eventRef)
        {
            if (FMODHelper.IsEvent3D(eventRef))
            {
                FMODHelper.PlaySound_3D_OneShot(eventRef, gameObject);
            }
            else
            {
                FMODHelper.PlaySound_2D_OneShot(eventRef);
            }

            OnSoundWasPlayed();
        }

        public virtual void PlayOneShot(EventReference eventRef, ParamRef parameter)
        {
            if (FMODHelper.IsEvent3D(eventRef))
            {
                FMODHelper.PlaySound_3D_OneShot(eventRef, gameObject, parameter);
            }
            else
            {
                FMODHelper.PlaySound_2D_OneShot(eventRef);
            }

            OnSoundWasPlayed();
        }

        public virtual void PlayOneShot(EventReference eventRef, ParamRef[] parameters)
        {
            if (FMODHelper.IsEvent3D(eventRef))
            {
                FMODHelper.PlaySound_3D_OneShot(eventRef, gameObject, parameters);
            }
            else
            {
                FMODHelper.PlaySound_2D_OneShot(eventRef);
            }

            OnSoundWasPlayed();
        }


        public virtual EventInstance PlaySound(EventReference eventRef)
        {
            EventInstance instance;
            if (FMODHelper.IsEvent3D(eventRef))
            {
                instance = FMODHelper.PlaySound_3D(eventRef, gameObject);
 
            }
            else
            {
                instance = FMODHelper.PlaySound_2D(eventRef);
            }

            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);
            return instance;
        }

        public virtual EventInstance PlaySound(EventReference eventRef, ParamRef parameter)
        {
            EventInstance instance;
            if (FMODHelper.IsEvent3D(eventRef))
            {
                instance = FMODHelper.PlaySound_3D(eventRef, gameObject, parameter);
            }
            else
            {
                instance = FMODHelper.PlaySound_2D(eventRef, parameter);
            }

            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);
            return instance;
        }

        public virtual EventInstance PlaySound(EventReference eventRef, ParamRef[] parameters)
        {
            EventInstance instance;
            if (FMODHelper.IsEvent3D(eventRef))
            {
                instance = FMODHelper.PlaySound_3D(eventRef, gameObject, parameters);
            }
            else
            {
                instance = FMODHelper.PlaySound_2D(eventRef, parameters);
            }

            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);
            return instance;
        }


        public virtual void SetParameter(EventInstance eventInstance, ParamRef parameter)
        {
            eventInstance.setParameterByName(parameter.Name, parameter.Value);
        }

        public virtual void SetParameter(EventInstance eventInstance, ParamRef[] parameters)
        {
            foreach(ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value);
            }
        }


        public virtual void StopSound(EventInstance instance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
        {
            if (!instance.isValid())
                return;

           OnSoundWillStopped(instance);
            instance.stop(stopMode);
        }

        public virtual void PauseSound(EventInstance instance, bool value)
        {
            if (!instance.isValid())
            {
                if (eventInstances.Contains(instance))
                    eventInstances.Remove(instance);
            }


            OnSoundWillPaused(instance);
            instance.setPaused(value);
        }

        #endregion

        #region Callbacks

        public virtual void OnSoundWasPlayed()
        {

        }

        public virtual void OnSoundWasPlayed(EventInstance instance)
        {

        }

        public virtual void OnSoundWillStopped(EventInstance instance)
        {

        }

        public virtual void OnSoundWillPaused(EventInstance instance)
        {

        }

        public virtual void OnSoundWasResumed(EventInstance instance)
        {

        }

        #endregion
    }

}

