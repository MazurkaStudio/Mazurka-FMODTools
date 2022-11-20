using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MazurkaGameKit.FMODTools
{
    [RequireComponent(typeof(HingeJoint2D))]
    public class FMODHingeJoint2DController : MonoBehaviour
    {
        [SerializeField] private FMODAudioEmitter audioEmitter;
        [SerializeField] private FMODPhysics2DHelper.DynamicValueSound[] frictionEvents;
        [SerializeField] private FMODPhysics2DHelper.JoinLimitsSound[] limitsEvents;
        
        [SerializeField] private bool emitSoundOnBreak;
        [SerializeField, ShowIf("emitSoundOnBreak")] private EventReference breakSound;
        
        private HingeJoint2D hingeJoint;
        
        private float lastSpeedValue;
        
        private void Awake()
        {
            hingeJoint = GetComponent<HingeJoint2D>();

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
            if (hingeJoint == null)
            {
                if (emitSoundOnBreak)
                    audioEmitter.PlayOneShot(breakSound);
                
                Destroy(this);
                return;
            }
            
            CheckJointFrictions();
            CheckJointLimits();
            
            lastSpeedValue = hingeJoint.jointSpeed;
        }

        private void CheckJointFrictions()
        {
            foreach (var friction in frictionEvents)
            {
                friction.EvaluateValue(hingeJoint.jointSpeed);
            }
        }

        private void CheckJointLimits()
        {
            foreach (var limit in limitsEvents)
            {
                limit.CheckLimit(hingeJoint.limitState, lastSpeedValue);
            }
        }
    }

}

