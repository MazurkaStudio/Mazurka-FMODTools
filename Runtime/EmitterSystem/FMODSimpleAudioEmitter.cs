using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    /// <summary>
    /// Base class to use audio emitter in static emitter event system
    /// Simple emitter can manage one event a the time
    /// </summary>
    public class FMODSimpleAudioEmitter : FMODAudioEmitter, IFMODAudioEmitter
    {
        [SerializeField] protected bool killOlder = true;
        [SerializeField] protected FMOD.Studio.STOP_MODE stopMode;

        #region Interface

        #region Emitter Registration

        public override void RegisterEmitter() => FMODAudioEmitterManager.Register(this);

        public override void UnregisterEmitter() => FMODAudioEmitterManager.Unregister(this);

        #endregion

        #region Event Instances Registration

        public EventInstance GetEventInstance => eventInstance;

        protected EventInstance eventInstance;

        public override void RegisterNewEventInstance(EventInstance eventInstance)
        {
            if (!eventInstance.isValid())
                return;

            if (killOlder)
                if (this.eventInstance.isValid())
                    this.eventInstance.stop(stopMode);

            this.eventInstance = eventInstance;

            IsPlaying = true;
        }

        public override void UnregisterNewEventInstance(EventInstance eventInstance)
        {
            IsPlaying = false;
            this.eventInstance = default;
        }

        public override void RefreshEventsInstances() 
        {
            if (!eventInstance.isValid())
                this.eventInstance = default;
        }

        #endregion

        protected override bool CanPlayNewSound()
        {
            if (!base.CanPlayNewSound())
                return false;

            if(!killOlder)
                if (this.eventInstance.isValid())
                    return false;

            return true;
        }

        public override void StopEmitter(bool allowFadeOut = true)
        {
            FMOD.Studio.STOP_MODE stopMode = allowFadeOut ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE;
            StopSound(eventInstance, stopMode);
        }

        public override void PauseEmitter(bool value)
        {
            if (canBePaused)
            {
                isInPause = value;

                if (value)
                {
                    OnSoundWillPaused(eventInstance);
                    eventInstance.setPaused(value);
                }
                else
                {
                    eventInstance.setPaused(value);
                    OnSoundWasResumed(eventInstance);
                }
            }
        }

        #endregion

        #region Callbacks

        protected override void OnSoundWasPlayed()
        {

        }

        protected override void OnSoundWasPlayed(EventInstance instance)
        {

        }

        protected override void OnSoundWillStopped(EventInstance instance)
        {

        }

        protected override void OnSoundWillPaused(EventInstance instance)
        {

        }

        protected override void OnSoundWasResumed(EventInstance instance)
        {

        }

        #endregion

        public void SetParameter(ParamRef parameter)
        {
            if (eventInstance.isValid())
                eventInstance.setParameterByName(parameter.Name, parameter.Value);
        }
    }

}

