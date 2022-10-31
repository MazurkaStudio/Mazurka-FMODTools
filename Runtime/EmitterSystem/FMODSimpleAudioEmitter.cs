using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace MazurkaGameKit.FMODTools
{
    /// <summary>
    /// Base class to use audio emitter in static emitter event system
    /// Simple emitter can manage one event a the time
    /// </summary>
    public class FMODSimpleAudioEmitter : FMODAudioEmitter, IFMODAudioEmitter
    {
        [SerializeField] private bool killOlder = true;
        [SerializeField] private FMOD.Studio.STOP_MODE stopMode;

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
            {
                if (this.eventInstance.isValid())
                    this.eventInstance.stop(stopMode);

                this.eventInstance = eventInstance;
            }
        }

        public override void UnregisterNewEventInstance(EventInstance eventInstance) { this.eventInstance = default; }

        public override void RefreshEventsInstances() 
        {
            if (!eventInstance.isValid())
                this.eventInstance = default;
        }

        #endregion

        public override bool CanPlayNewSound()
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

