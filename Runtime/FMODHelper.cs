using System;
using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using FMOD;
using UnityEngine.Events;

namespace MazurkaGameKit.FMODTools
{
    public static class FMODHelper
    {

        public static EventInstance CreateInstance(EventReference eventRef)
        {
            if (eventRef.IsNull)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning("You are trying create an instance from null event reference");
#endif
                return default;
            }

            return RuntimeManager.CreateInstance(eventRef);
        }


        #region Global Parameters

        /// <summary>
        /// Change global FMOD parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <param name="ignoreKeepSpeed"></param>
        public static void ChangeGlobaleParameter(string parameter, float value, bool ignoreKeepSpeed = false)
        {
            RuntimeManager.StudioSystem.setParameterByName(parameter, value, ignoreKeepSpeed);
        }

        /// <summary>
        /// Change global FMOD parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="labelValue"></param>
        /// <param name="ignoreKeepSpeed"></param>
        public static void ChangeGlobaleParameter(string parameter, string labelValue, bool ignoreKeepSpeed = false)
        {
            RuntimeManager.StudioSystem.setParameterByNameWithLabel(parameter, labelValue, ignoreKeepSpeed);
        }

        #endregion


        #region Event Details

        /// <summary>
        /// This function return the timeline duration in seconds, not really the sound lenght, only work on linear straight sound
        /// </summary>
        /// <param name="eventInstance"></param>
        /// <param name="doAtEnd"></param>
        /// <returns></returns>
        public static float GetEventTimelineLenght(EventInstance eventInstance)
        {
            if (eventInstance.getDescription(out EventDescription de) == RESULT.OK)
                if (de.getLength(out int lenght) == RESULT.OK)
                    return (lenght * 1f) / 100f;

            return -1f;
        }

        public static EventDescription GetEventDescription(EventReference eventRef)
        {
            return RuntimeManager.GetEventDescription(eventRef);
        }

        public static bool GetEventDescription(EventInstance eventInstance, out EventDescription eventDescription)
        {
            if (eventInstance.getDescription(out eventDescription) == RESULT.OK)
                return true;

            return false;
        }

        public static bool IsEvent3D(EventReference eventRef)
        {
            GetEventDescription(eventRef).is3D(out bool is3D);
            return is3D;
        }

        public static bool IsEvent3D(EventInstance eventInstance)
        {
            bool is3D = false;

            if (GetEventDescription(eventInstance, out EventDescription descr))
                descr.is3D(out is3D);

            return is3D;
        }

        #endregion


        #region Play Sound

        public static void PlayUISound_OneShot(EventReference eventRef)
        {
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            PlaySound_2D_OneShot(eventRef);
        }
        
        #region One Shot

        public static bool PlaySound_OneShot(EventReference eventRef, GameObject source)
        {
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                PlaySound_3D_OneShot(eventRef, source);
            }
            else
            {
                PlaySound_2D_OneShot(eventRef);
            }

            return true;
        }

        public static bool PlaySound_OneShot(EventReference eventRef, Vector3 source)
        {
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                PlaySound_3D_OneShot(eventRef, source);
            }
            else
            {
                PlaySound_2D_OneShot(eventRef);
            }
            
            return true;
        }
        
        public static bool PlaySound_OneShot(EventReference eventRef, GameObject source, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                PlaySound_3D_OneShot(eventRef, source, parameter, ignoreKeepSpeed);
            }
            else
            {
                PlaySound_2D_OneShot(eventRef, parameter, ignoreKeepSpeed);
            }
            
            return true;
        }

        public static bool PlaySound_OneShot(EventReference eventRef, GameObject source, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                PlaySound_3D_OneShot(eventRef, source, parameters, ignoreKeepSpeed);
            }
            else
            {
                PlaySound_2D_OneShot(eventRef, parameters, ignoreKeepSpeed);
            }
            
            return true;
        }
        
        public static bool PlaySound_OneShot(EventReference eventRef, Vector3 source, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                PlaySound_3D_OneShot(eventRef, source, parameter, ignoreKeepSpeed);
            }
            else
            {
                PlaySound_2D_OneShot(eventRef, parameter, ignoreKeepSpeed);
            }
            
            return true;
        }

        public static bool PlaySound_OneShot(EventReference eventRef, Vector3 source, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                PlaySound_3D_OneShot(eventRef, source, parameters, ignoreKeepSpeed);
            }
            else
            {
                PlaySound_2D_OneShot(eventRef, parameters, ignoreKeepSpeed);
            }
            
            return true;
        }
        
        #region Private

        private static void PlaySound_2D_OneShot(EventReference eventRef)
        {
            RuntimeManager.PlayOneShot(eventRef);
        }

        private static void PlaySound_3D_OneShot(EventReference eventRef, Vector3 position)
        {
            RuntimeManager.PlayOneShot(eventRef, position);
        }

        private static void PlaySound_3D_OneShot(EventReference eventRef, GameObject attachTo)
        { 
            RuntimeManager.PlayOneShotAttached(eventRef, attachTo);
        }


        private static void PlaySound_2D_OneShot(EventReference eventRef, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            eventInstance.start();
            eventInstance.release();
        }

        private static void PlaySound_2D_OneShot(EventReference eventRef, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            foreach (ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            }
            eventInstance.start();
            eventInstance.release();
        }


        private static void PlaySound_3D_OneShot(EventReference eventRef, Vector3 position, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            eventInstance.start();
            eventInstance.release();
        }

        private static void PlaySound_3D_OneShot(EventReference eventRef, GameObject attachTo, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            RuntimeManager.AttachInstanceToGameObject(eventInstance, attachTo.transform, attachTo.GetComponent<Rigidbody2D>());
            eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            eventInstance.start();
            eventInstance.release();
        }

        private static void PlaySound_3D_OneShot(EventReference eventRef, Vector3 position, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            foreach (ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            }

            eventInstance.start();
            eventInstance.release();

        }

        private static void PlaySound_3D_OneShot(EventReference eventRef, GameObject attachTo, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            RuntimeManager.AttachInstanceToGameObject(eventInstance, attachTo.transform, attachTo.GetComponent<Rigidbody2D>());
            foreach (ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            }

            eventInstance.start();
            eventInstance.release();
        }
        
        #endregion

        #endregion

        #region Instance

        public static bool PlaySound(EventReference eventRef, GameObject source, out EventInstance eventInstance)
        {
            eventInstance = default;
            
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                eventInstance = PlaySound_3D(eventRef, source);
            }
            else
            {
                eventInstance = PlaySound_2D(eventRef);
            }

            return true;
        }

        public static bool PlaySound(EventReference eventRef, Vector3 source, out EventInstance eventInstance)
        {
            eventInstance = default;
            
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                eventInstance = PlaySound_3D(eventRef, source);
            }
            else
            {
                eventInstance = PlaySound_2D(eventRef);
            }
            
            return true;
        }


        public static bool PlaySound(EventReference eventRef, GameObject source, ParamRef parameter, out EventInstance eventInstance, bool ignoreKeepSpeed = false)
        {
            eventInstance = default;
            
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                eventInstance = PlaySound_3D(eventRef, source, parameter, ignoreKeepSpeed);
            }
            else
            {
                eventInstance = PlaySound_2D(eventRef, parameter, ignoreKeepSpeed);
            }
            
            return true;
        }

        public static bool PlaySound(EventReference eventRef, GameObject source, ParamRef[] parameters, out EventInstance eventInstance, bool ignoreKeepSpeed = false)
        {
            eventInstance = default;
            
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                eventInstance = PlaySound_3D(eventRef, source, parameters, ignoreKeepSpeed);
            }
            else
            {
                eventInstance = PlaySound_2D(eventRef, parameters, ignoreKeepSpeed);
            }
            
            return true;
        }


        public static bool PlaySound(EventReference eventRef, Vector3 source, ParamRef parameter, out EventInstance eventInstance, bool ignoreKeepSpeed = false)
        {
            eventInstance = default;
            
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif
            
            if (IsEvent3D(eventRef))
            {
                eventInstance = PlaySound_3D(eventRef, source, parameter, ignoreKeepSpeed);
            }
            else
            {
                eventInstance = PlaySound_2D(eventRef, parameter, ignoreKeepSpeed);
            }
            
            return true;
        }

        public static bool PlaySound(EventReference eventRef, Vector3 source, ParamRef[] parameters, out EventInstance eventInstance, bool ignoreKeepSpeed = false)
        {
            eventInstance = default;
            
#if UNITY_EDITOR
            if (eventRef.IsNull) throw new Exception("Your trying playing a null event");
#else
            if (eventRef.IsNull) return false;
#endif

            if (IsEvent3D(eventRef))
            {
                eventInstance = PlaySound_3D(eventRef, source, parameters, ignoreKeepSpeed);
            }
            else
            {
                eventInstance = PlaySound_2D(eventRef, parameters, ignoreKeepSpeed);
            }
            
            return true;
        }


        #region Private

        private static EventInstance PlaySound_2D(EventReference eventRef)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.start();
            return eventInstance;
        }

        private static EventInstance PlaySound_3D(EventReference eventRef, Vector3 position)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            eventInstance.start();
            return eventInstance;
        }

        private static EventInstance PlaySound_3D(EventReference eventRef, GameObject attachTo)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            RuntimeManager.AttachInstanceToGameObject(eventInstance, attachTo.transform);
            eventInstance.start();
            return eventInstance;
        }


        private static EventInstance PlaySound_2D(EventReference eventRef, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            eventInstance.start();
            return eventInstance;
        }

        private static EventInstance PlaySound_2D(EventReference eventRef, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            foreach (ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            }
            eventInstance.start();
            return eventInstance;
        }


        private static EventInstance PlaySound_3D(EventReference eventRef, Vector3 position, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            eventInstance.start();
            return eventInstance;
        }

        private static EventInstance PlaySound_3D(EventReference eventRef, GameObject attachTo, ParamRef parameter, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            RuntimeManager.AttachInstanceToGameObject(eventInstance, attachTo.transform, attachTo.GetComponent<Rigidbody2D>());
            eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            eventInstance.start();
            return eventInstance;
        }

        private static EventInstance PlaySound_3D(EventReference eventRef, Vector3 position, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));

            foreach(ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            }

            eventInstance.start();
            return eventInstance;
        }

        private static EventInstance PlaySound_3D(EventReference eventRef, GameObject attachTo, ParamRef[] parameters, bool ignoreKeepSpeed = false)
        {
            EventInstance eventInstance = CreateInstance(eventRef);
            RuntimeManager.AttachInstanceToGameObject(eventInstance, attachTo.transform, attachTo.GetComponent<Rigidbody2D>());
            foreach (ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value, ignoreKeepSpeed);
            }
            eventInstance.start();
            return eventInstance;
        }

        #endregion
        
        #endregion

        #endregion


        #region Track Sound

        //TODO : Test

        public static IEnumerator WaitForEventInstanceStop(UnityAction callback, EventInstance instance)
        {
            WaitWhile wait = new WaitWhile(() => EventInstanceIsPlaying(instance));
            yield return wait;
            callback();
        }

        public static bool EventInstanceIsPlaying(EventInstance instance)
        {
            instance.getPlaybackState(out PLAYBACK_STATE state);
            return state != PLAYBACK_STATE.STOPPED;
        }


        public static bool IsEventInstancePlaying(EventInstance instance)
        {
            PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            return state != PLAYBACK_STATE.STOPPED;
        }

        #endregion
    }

    /// <summary>
    /// Use for triggering FMOD event from monobehavior events
    /// </summary>
    [System.Serializable]
    public enum StartSoundEvent
    {
        OnEnable, OnStart, OnDisable, Custom
    }

    /// <summary>
    /// Possible action on an audio actor
    /// </summary>
    [System.Serializable]
    public enum ControlAction
    {
        StartEvent,
        StopEvent,
        UpdateEventParameters
    }
}


