using FMOD.Studio;
using System.Collections.Generic;

namespace MazurkaGameKit.FMODTools
{
    /// <summary>
    /// Base class to use audio emitter in static emitter event system
    /// </summary>
    public class FMODComplexAudioEmitter : FMODAudioEmitter, IFMODAudioEmitter
    {
        #region Interface

        #region Emitter Registration

        public override void RegisterEmitter() => FMODAudioEmitterManager.Register(this);

        public override void UnregisterEmitter() => FMODAudioEmitterManager.Unregister(this);

        #endregion

        #region Event Instances Registration

        public List<EventInstance> GetEventInstances => eventInstances;

        protected List<EventInstance> eventInstances = new List<EventInstance>();

        public override void RegisterNewEventInstance(EventInstance eventInstance)
        {
            if (!eventInstance.isValid())
                return;

            if (eventInstances.Contains(eventInstance))
                return;

            eventInstances.Add(eventInstance);
            
            IsPlaying = false;
        }

        public override void UnregisterNewEventInstance(EventInstance eventInstance)
        {
            if (!eventInstances.Contains(eventInstance))
                return;

            eventInstances.Remove(eventInstance);

            if (eventInstances.Count == 0)
            {
                IsPlaying = false;
            }
        }

        public override void RefreshEventsInstances()
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


        public override void StopEmitter(bool allowFadeOut = true)
        {
            FMOD.Studio.STOP_MODE stopMode = allowFadeOut ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE;

            for (int i = eventInstances.Count - 1; i >= 0; i--)
            {
                StopSound(eventInstances[i], stopMode);
            }

            eventInstances.Clear();
        }

        public override void PauseEmitter(bool value)
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

        #region Callbacks

        public override void OnSoundWasPlayed()
        {

        }

        public override void OnSoundWasPlayed(EventInstance instance)
        {

        }

        public override void OnSoundWillStopped(EventInstance instance)
        {

        }

        public override void OnSoundWillPaused(EventInstance instance)
        {

        }

        public override void OnSoundWasResumed(EventInstance instance)
        {

        }

        #endregion
    }

}

