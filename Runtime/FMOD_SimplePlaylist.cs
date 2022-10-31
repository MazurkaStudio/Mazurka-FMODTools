using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    public class FMOD_SimplePlaylist : MonoBehaviour
    {
        #region Vars

        [Header("PLAYLIST--------------------------------------------------------------------------------------------")]
        [SerializeField] protected List<EventReference> playlist;
        [SerializeField] protected Transform soundSource;
        [SerializeField] protected bool playOnStart;
        [SerializeField] protected bool stopPlaylistIfDestroyed = true;


        [Header("SETTINGS--------------------------------------------------------------------------------------------")]
        [SerializeField] protected bool loop;
        [SerializeField] protected bool randomize;  
        [SerializeField] protected FMOD.Studio.STOP_MODE stopMode;
   

        [Header("WAIT------------------------------------------------------------------------------------------------")]
        [SerializeField] protected bool waitBetweenEvents;
        [SerializeField] protected Vector2 minMaxWaitDuration;

        #endregion

        protected EventInstance currentEventInstance;
        protected EventReference currentEventRef;
        protected float waitingTime;
        protected float lastWaitRandomDuration;

        public bool IsPlaying { get; protected set; }
        public bool IsStoping { get; protected set; }
        public bool IsWaiting { get; protected set; }
        public bool IsInPause { get; protected set; }

        public int CurrentEventIndex { get; protected set; }

        #region Main Fonctions

        public virtual void AddToPlaylist(EventReference eventRef)
        {
            playlist.Add(eventRef);
        }

        public virtual void StartPlaylist()
        {
            if(IsInPause)
            {
                if (currentEventInstance.isValid())
                {
                    currentEventInstance.setPaused(false);
                }

                IsInPause = false;
            }
            else
            {
                CurrentEventIndex = randomize ? Random.Range(0, playlist.Count) : 0;
                PlayNextEvent();
            }
        }

        public virtual void PausePlaylist()
        {
            if(IsInPause)
            {
                StartPlaylist();
                return;
            }

            IsInPause = true;
            
            if(currentEventInstance.isValid())
            {
                currentEventInstance.setPaused(true);
            }
        }

        public virtual void StopPlaylist()
        {
            IsWaiting = false;
            IsPlaying = false;
            IsStoping = true;
            CurrentEventIndex = randomize ? GetRandomValue() : 0;
            currentEventInstance.stop(stopMode);
        }

        #endregion

        #region Logic

        protected void Start()
        {
            if (playOnStart)
                StartPlaylist();
        }

        protected void PlayNextEvent()
        {
            if(CurrentEventIndex < playlist.Count)
            {
                StartCurrentEvent();
            }
            else
            {
                if(loop)
                {
                    CurrentEventIndex = randomize ? GetRandomValue() : 0;

                    if (waitBetweenEvents)
                    {
                        GoToWait();
                    }
                    else
                    {
                        StartCurrentEvent();
                    }
                }
                else
                {
                    IsPlaying = false;
                    IsWaiting = false;
                    IsStoping = false;
                }
            }
        }

        protected void StartCurrentEvent()
        {
            currentEventRef = playlist[CurrentEventIndex];
            RuntimeManager.GetEventDescription(currentEventRef).is3D(out bool is3D);

            if (is3D)
            {
                currentEventInstance = FMODHelper.PlaySound_3D(currentEventRef, soundSource.gameObject);
            }
            else
            {
                currentEventInstance = FMODHelper.PlaySound_2D(currentEventRef);
            }

            IsPlaying = true;
            IsWaiting = false;
            IsStoping = false;
        }

        protected void GoToWait()
        {
            waitingTime = 0f;
            lastWaitRandomDuration = Random.Range(minMaxWaitDuration.x, minMaxWaitDuration.y);
            IsWaiting = true;
            IsPlaying = false;
        }

        protected void Update()
        {
            if (IsInPause)
                return;

            if(IsPlaying || IsStoping)
            {
                if(!FMODHelper.IsEventInstancePlaying(currentEventInstance))
                {
                    OnEventStop();
                }
            }
            else if(IsWaiting)
            {
                waitingTime += Time.deltaTime;

                if(waitingTime >= lastWaitRandomDuration)
                {
                    IsWaiting = false;                  
                    PlayNextEvent();
                }
            }
        }

        protected virtual void OnEventStop()
        {
            CurrentEventIndex = randomize ? GetRandomValue() : CurrentEventIndex + 1;

            if (!IsStoping)
            {
                if (waitBetweenEvents)
                {
                    GoToWait();
                }
                else
                {
                    PlayNextEvent();
                }
            }
        }

        protected void OnDestroy()
        {
            if (stopPlaylistIfDestroyed)
            {
                StopPlaylist();
            }
        }

        #endregion

        #region Utils

        const int MAX_RANDOM_ITERATION = 6;

        protected virtual int GetRandomValue()
        {
            int randomValue = CurrentEventIndex;

            int iteration = 0;

            while(CurrentEventIndex == randomValue)
            {
                randomValue = Random.Range(0, playlist.Count);
                iteration++;

                if (iteration > MAX_RANDOM_ITERATION)
                    break;
            }

            return randomValue;
        }

        #endregion


#if UNITY_EDITOR

        protected void Reset()
        {
            if (soundSource == null)
                soundSource = transform;
        }
#endif
    }
}

