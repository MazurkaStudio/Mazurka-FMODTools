using UnityEngine;
using UnityEngine.Playables;
using FMODUnity;

namespace MazurkaGameKit.FMODTools
{
    public class FMOD_TimelineEventAsset : PlayableAsset
    {
/*        public EventTag eventTag;
        public ControlAction controlAction;*/
        public ParamRef[] parameters;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<FMOD_TimelineEventBehaviour>.Create(graph);
            var audioEventBehaviour = playable.GetBehaviour();
/*            audioEventBehaviour.eventTag = eventTag;
            audioEventBehaviour.controlAction = controlAction;*/
            audioEventBehaviour.parameters = parameters;
            return playable;
        }
    }
}