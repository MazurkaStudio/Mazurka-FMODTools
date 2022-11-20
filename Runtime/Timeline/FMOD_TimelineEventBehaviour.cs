using FMODUnity;
using UnityEngine.Playables;

namespace MazurkaGameKit.FMODTools
{
    public class FMOD_TimelineEventBehaviour : PlayableBehaviour
    {
  /*      public EventTag eventTag;
        public ControlAction controlAction;*/
        public ParamRef[] parameters;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
  /*          if (eventTag != null)
            {
                EventManager.PostEvent(new ControlActionEventArguments(eventTag, controlAction, parameters));
            }*/
        }
    }
}