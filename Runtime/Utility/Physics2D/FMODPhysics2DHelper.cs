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
    public static class FMODPhysics2DHelper
    {
        [System.Serializable]
        public class JoinLimitsSound
        {
            private FMODAudioEmitter audioEmitter;
            
            [SerializeField] private JointLimitState2D limitType = JointLimitState2D.EqualLimits;
            [SerializeField] private EventReference limitSoundEvent;
            [SerializeField] private float maxLimitForce;
            [Tooltip("Min relative force to trigger sound (relative to maxLimitForce)"), SerializeField, Range(0f, 1f)] private float tresholdRelativeForce;
            [Tooltip("Send relative force parameters when sound is trigger"),SerializeField] private bool useRelativeForce;
          
            [SerializeField, ShowIf("useRelativeForce")] private string relativeForceParamName;
            
            private ParamRef relativeForceParam;

            private bool haveReachLimit;

            public UnityAction onLimitWasReached;

            public void Initialize(FMODAudioEmitter audioEmitter)
            {
                this.audioEmitter = audioEmitter;
                relativeForceParam = new ParamRef { Name = relativeForceParamName };
            }

            public void CheckLimit(JointLimitState2D jointLimitState, float deltaSpeed)
            {
                if (jointLimitState == limitType)
                {
                    if (haveReachLimit)
                        return;

                    float relativeForce = Mathf.Abs(deltaSpeed * Time.fixedDeltaTime) / (maxLimitForce * Time.fixedDeltaTime);
                    
                    if (relativeForce < tresholdRelativeForce)
                        return;
                    
                    haveReachLimit = true;
                    onLimitWasReached?.Invoke();

                    if (useRelativeForce)
                    {
                        relativeForceParam.Value = relativeForce;
                        audioEmitter.PlayOneShot(limitSoundEvent, relativeForceParam);
                    }
                    else
                    {
                        audioEmitter.PlayOneShot(limitSoundEvent);
                    }
                   
                }
                else
                {
                    haveReachLimit = false;
                }
            }
        }
        
        [System.Serializable]
        public class DynamicValueSound
        { 
            private FMODAudioEmitter audioEmitter;
            
            [SerializeField] private float maxDynamicValue = 50f;
            [SerializeField, Range(0f, 1f)] private float dynamicRelativeTresholdValue = 0.05f;
            [SerializeField] private string dynamicValueParamName = "speed";
            [SerializeField] private EventReference dynamicValueSoundEvent;
            
            private EventInstance dynamicValueSoundInstance;
            private ParamRef dynmaicValueParam;

            private bool isEmittingSound;
            private float currentRelativeValue;
            
            public void Initialize(FMODAudioEmitter audioEmitter)
            {
                this.audioEmitter = audioEmitter;
                dynmaicValueParam = new ParamRef { Name = dynamicValueParamName };
            }

            public float EvaluateValue(float valueDelta)
            {
                currentRelativeValue = Mathf.Clamp01(Mathf.Abs(valueDelta * Time.fixedDeltaTime) / (maxDynamicValue * Time.fixedDeltaTime));
                dynmaicValueParam.Value = currentRelativeValue;
                audioEmitter.SetParameter(dynamicValueSoundInstance, dynmaicValueParam);
                
                CheckIfNeedEmitSound();
                
                return currentRelativeValue;
            }

            public void CheckIfNeedEmitSound()
            {
                if (isEmittingSound)
                {
                    if (currentRelativeValue < dynamicRelativeTresholdValue)
                    {
                        audioEmitter.StopSound(dynamicValueSoundInstance);
                        isEmittingSound = false;
                    }
                }
                else
                {
                    if (currentRelativeValue >= dynamicRelativeTresholdValue)
                    {
                        audioEmitter.PlaySound(dynamicValueSoundEvent, out dynamicValueSoundInstance);
                        isEmittingSound = true;
                    }
                }
            }

            public void ForceStop()
            {
                audioEmitter.StopSound(dynamicValueSoundInstance);
                isEmittingSound = false;
            }
        }

        [System.Serializable]
        public class Collision2DSound
        {
            private FMODAudioEmitter audioEmitter;
            
            [SerializeField] private EventReference collisionSoundEvent;
            [SerializeField] private float maxCollisionForce = 50f;
            [SerializeField] private float collisionTreshold = 2f;

            [SerializeField] private bool isCollisionRelative;
            [SerializeField, ShowIf("isCollisionRelative")] private string collisionRelativeParamName = "collisionRelativeForce";
            private ParamRef collisionParam;
            
            [SerializeField] private bool useMinTimeBetweenSound;
            [SerializeField, ShowIf("useMinTimeBetweenSound")] private float minTimeBetweenSound = 0.1f;
            private float lastSoundTime;
            public void Initialize(FMODAudioEmitter audioEmitter)
            {
                this.audioEmitter = audioEmitter;
                collisionParam = new ParamRef { Name = collisionRelativeParamName };
            }

            public void EvaluateCollision(float collisionForce)
            {
                if (collisionForce < collisionTreshold)
                    return;

                if (useMinTimeBetweenSound && Time.time - lastSoundTime < minTimeBetweenSound)
                    return;

                if (isCollisionRelative)
                {
                    collisionParam.Value = Mathf.Clamp01(collisionForce / maxCollisionForce);
                    audioEmitter.PlayOneShot(collisionSoundEvent, collisionParam);
                }
                else
                {
                    audioEmitter.PlayOneShot(collisionSoundEvent);
                }
                
                lastSoundTime = Time.time;
            }
        }
    }

}

