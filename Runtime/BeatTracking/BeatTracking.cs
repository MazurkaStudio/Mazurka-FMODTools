using System;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace MazurkaGameKit.FMODTools
{
    /// <summary>
    /// Start a music and subscribe to the beat and marker events to track music events
    /// </summary>
    public class BeatTracking : MonoBehaviour
    {
        private string lastMarkerName;
        private int lastBeat;
        private TimelineInfo timelineInfo;
        private GCHandle timelineHandle;
        private EVENT_CALLBACK beatCallback;
        private EventInstance musicInstance;
        
        
        public static UnityAction<int> onBeatChanged;
        public static UnityAction<string> onMarkerChanged;
        public static UnityAction onMusicEnd;
        
        public static BeatTracking Instance {get; private set;}
        public bool IsPlayingMusic { get; private set; }
        
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }
        
        private void Update()
        {

            if (!IsPlayingMusic) return;

            if (!IsEventIsValid())
            {
                StopBeatTrackedMusic();
                return;
            }
            
            if(lastMarkerName != timelineInfo.lastMarker)
            {
                lastMarkerName = timelineInfo.lastMarker;
                onMarkerChanged?.Invoke(lastMarkerName);
            }
            
            if(lastBeat != timelineInfo.currentBeat)
            {
                lastBeat = timelineInfo.currentBeat;
                onBeatChanged?.Invoke(lastBeat);
            }
        }

        private void OnDestroy()
        {
            musicInstance.setUserData(IntPtr.Zero);
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
            timelineHandle.Free();
        }
        
        
        
        public void StartBeatTrackedMusic(EventReference music)
        {
            if (IsPlayingMusic) return;

            IsPlayingMusic = true;
            
            musicInstance = FMODHelper.CreateInstance(music);
            musicInstance.start();
            
            timelineInfo = new TimelineInfo();
            
            beatCallback = new EVENT_CALLBACK(BeatEventCallback);
            
            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);

            musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
            musicInstance.setCallback(beatCallback, EVENT_CALLBACK_TYPE.TIMELINE_MARKER | EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
        }
        
        public void StopBeatTrackedMusic()
        {
            if (!IsPlayingMusic) return;
            
            musicInstance.setUserData(IntPtr.Zero);
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
            timelineHandle.Free();
            
            onMusicEnd?.Invoke();
            
            IsPlayingMusic = false;
        }
        
        private bool IsEventIsValid()
        {
            PLAYBACK_STATE state;
            musicInstance.getPlaybackState(out state);
            return state != PLAYBACK_STATE.STOPPED;
        }
        
        
        [StructLayout(LayoutKind.Sequential)]
        public class TimelineInfo
        {
            public int currentBeat = 0;
            public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
        }
        
        [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
        static FMOD.RESULT BeatEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
        {
            EventInstance instance = new EventInstance(instancePtr);

            IntPtr timelineInfoPtr;
            FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

            if (result != FMOD.RESULT.OK)
            {
                Debug.LogError(("TimelineInfo Callback Error" + result));
            }
            else if (timelineInfoPtr != IntPtr.Zero)
            {
                GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
                TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

                switch (type)
                {
                    case EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                        break;
                    case EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBeat = parameter.beat;
                    }
                        break;
                }
            }
            return FMOD.RESULT.OK;
        }
    }
}
