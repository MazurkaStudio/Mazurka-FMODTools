using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    /// <summary>
    /// Use this interface to create Audio Emitter that work with static emitter event system
    /// </summary>
    public interface IFMODAudioEmitter
    {
        #region Emitter registration

        /// <summary>
        /// Registe emitter to emitter static list onStart();
        /// You can use this list to send global events to all emitter (pause sound, kill instances, ...)
        /// </summary>
        public void RegisterEmitter();

        /// <summary>
        /// And unregister from static emitter list onDestroy();
        /// </summary>
        public void UnregisterEmitter();

        #endregion


        #region Event instances

        /// <summary>
        /// Add new event instance to instance list
        /// </summary>
        /// <param name="eventInstance"></param>
        public void RegisterNewEventInstance(EventInstance eventInstance);

        /// <summary>
        /// Remove event instance from instance list
        /// </summary>
        /// <param name="eventInstance"></param>
        public void UnregisterNewEventInstance(EventInstance eventInstance);

        /// <summary>
        /// Use to clear unvalid event instances
        /// To use before functions that iterate on all event instances
        /// </summary>
        public void RefreshEventsInstances();

        #endregion


        #region Global emitter events

        public void PauseEmitter(bool value);

        public void StopEmitter(bool allowFadeOut = true);

        #endregion


        #region Getters

        public GameObject GetSoundEmitter { get; }

        public bool IsInPause { get; }

        public bool CanEmitSound { get; }
        public bool IsPlaying { get; }

        #endregion
    }
}

