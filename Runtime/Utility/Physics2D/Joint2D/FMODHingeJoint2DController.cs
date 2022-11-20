using System;
using System.Collections;
using System.Collections.Generic;
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
        
        [BoxGroup("Joint Friction"), SerializeField] private bool emitSoundOnJointMovement = true;
        [BoxGroup("Joint Friction"), ShowIf("emitSoundOnJointMovement"), SerializeField] private float maxJointSpeed = 50f;
        [BoxGroup("Joint Friction"), ShowIf("emitSoundOnJointMovement"), SerializeField, Range(0f, 1f)] private float jointMovementTreshold = 0.05f;
        [BoxGroup("Joint Friction"), ShowIf("emitSoundOnJointMovement"), SerializeField] private string jointSpeedParamName = "jointSpeed";
        [BoxGroup("Joint Friction"), ShowIf("emitSoundOnJointMovement"), SerializeField] private EventReference jointMovementSoundEvent;
        
        private EventInstance jointMovementSoundInstance;
        private ParamRef jointSpeedParam;
        public float CurrentRelativeJointSpeed { get; private set; }
        private bool jointMovementIsEmitting;

  
        [BoxGroup("Max Angle"), SerializeField] private bool emitSoundOnMaxAngle = false;
        [BoxGroup("Max Angle"), ShowIf("emitSoundOnMaxAngle"), SerializeField] private EventReference onMaxAngleSoundEvent;
        [BoxGroup("Max Angle"), ShowIf("emitSoundOnMaxAngle"), SerializeField, Range(0f, 1f)]private float minRelativeForceForMaxAngleSound;
        
        private bool jointHaveReachMaxLimit;
        public UnityAction onReachMaxAngle;

        [BoxGroup("Min Angle"), SerializeField] private bool emitSoundOnMinAngle = false;
        [BoxGroup("Min Angle"), ShowIf("emitSoundOnMinAngle"), SerializeField] private EventReference onMinAngleSoundEvent;
        [BoxGroup("Min Angle"), ShowIf("emitSoundOnMinAngle"), SerializeField, Range(0f, 1f)] private float minRelativeForceForMinAngleSound;
        
        public UnityAction onReachMinAngle;
        private bool jointHaveReachMinLimit;
        
        private HingeJoint2D hingeJoint;
        
        
        private void Awake()
        {
            hingeJoint = GetComponent<HingeJoint2D>();
            jointSpeedParam = new ParamRef { Name = jointSpeedParamName };
        }

        private void FixedUpdate()
        {
            CurrentRelativeJointSpeed = Mathf.Clamp01((Mathf.Abs(hingeJoint.jointSpeed * Time.fixedDeltaTime)) / (maxJointSpeed * Time.fixedDeltaTime));
            jointSpeedParam.Value = CurrentRelativeJointSpeed;
            audioEmitter.SetParameter(jointMovementSoundInstance, jointSpeedParam);
            CheckJointMovementState();
            CheckJointLimits();
        }

        private void CheckJointMovementState()
        {
            if (!emitSoundOnJointMovement)
                return;
            
            if (jointMovementIsEmitting)
            {
                if (CurrentRelativeJointSpeed < jointMovementTreshold)
                {
                    audioEmitter.StopSound(jointMovementSoundInstance);
                    jointMovementIsEmitting = false;
                }
            }
            else
            {
                if (CurrentRelativeJointSpeed >= jointMovementTreshold)
                {
                    audioEmitter.PlaySound(jointMovementSoundEvent, out jointMovementSoundInstance);
                    jointMovementIsEmitting = true;
                }
            }
        }

        private void CheckJointLimits()
        {
            if (emitSoundOnMaxAngle)
            {
                if (hingeJoint.limitState == JointLimitState2D.UpperLimit)
                {  
                    if (jointHaveReachMaxLimit)
                        return;

                    if (CurrentRelativeJointSpeed < minRelativeForceForMaxAngleSound)
                        return;
                    
                    jointHaveReachMaxLimit = true;
                    onReachMaxAngle?.Invoke();
                    audioEmitter.PlayOneShot(onMaxAngleSoundEvent);
                }
                else
                {
                    jointHaveReachMaxLimit = false;
                }
            }

            if (emitSoundOnMinAngle)
            {
                if (hingeJoint.limitState == JointLimitState2D.LowerLimit)
                {
                    if (jointHaveReachMinLimit)
                        return;
                    
                    if (CurrentRelativeJointSpeed < minRelativeForceForMinAngleSound)
                        return;
                    
                    jointHaveReachMinLimit = true;
                    onReachMinAngle?.Invoke();
                    audioEmitter.PlayOneShot(onMinAngleSoundEvent);
                }
                else
                {
                    jointHaveReachMinLimit = false;
                }
            }
        }
    }

}

