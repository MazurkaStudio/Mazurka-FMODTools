using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MazurkaGameKit.FMODTools
{
    [RequireComponent(typeof(SliderJoint2D))]
    public class FMODSliderJoint2DController : MonoBehaviour
    {
        [SerializeField] private FMODAudioEmitter audioEmitter;
        [SerializeField] private FMODPhysics2DHelper.DynamicValueSound[] frictionEvents;
        [SerializeField] private FMODPhysics2DHelper.JoinLimitsSound[] limitsEvents;
        
        [SerializeField] private bool emitSoundOnBreak;
        [SerializeField, ShowIf("emitSoundOnBreak")] private EventReference breakSound;
        
        private SliderJoint2D sliderJoint;

        private float lastSpeedValue;

        private void Awake()
        {
            sliderJoint = GetComponent<SliderJoint2D>();
            foreach (var friction in frictionEvents)
            {
                friction.Initialize(audioEmitter);
            }
            foreach (var limit in limitsEvents)
            {
                limit.Initialize(audioEmitter);
            }
        }

        private void FixedUpdate()
        {
            if (sliderJoint == null)
            {
                if (emitSoundOnBreak)
                    audioEmitter.PlayOneShot(breakSound);
                
                Destroy(this);
                return;
            }
            
            CheckJointFrictions();
            CheckJointLimits();
            
            lastSpeedValue = sliderJoint.jointSpeed;
        }
        
        private void CheckJointFrictions()
        {
            foreach (var friction in frictionEvents)
            {
                friction.EvaluateValue(sliderJoint.jointSpeed);
            }
        }

        private void CheckJointLimits()
        {
            foreach (var limit in limitsEvents)
            {
                limit.CheckLimit(sliderJoint.limitState, lastSpeedValue);
            }
        }
    }
}
