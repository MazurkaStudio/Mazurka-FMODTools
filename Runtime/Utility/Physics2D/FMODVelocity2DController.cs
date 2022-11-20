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
    [RequireComponent(typeof(Rigidbody2D))]
    public class FMODVelocity2DController : MonoBehaviour
    {
        [SerializeField] private FMODAudioEmitter audioEmitter;

        [SerializeField] private bool emitSoundWhenMove = true;
        [SerializeField] private EventReference positionSoundEvent;
        private EventInstance positionSoundInstance;
        [SerializeField] private string positionParamName = "relativeSpeed";
        private ParamRef positionParam;
        [SerializeField] private float maxPositionSpeed = 10f;
        [SerializeField, Range(0f, 1f)] private float positionTreshold= 0.05f;
        public bool IsEmittingPositionSound { get; private set; }
        public float CurrentRelativePosition { get; private set; }
        
        [SerializeField] private bool emitSoundWhenRotate;
        [SerializeField] private EventReference rotationSoundEvent;
        private EventInstance rotationSoundInstance;
        [SerializeField] private string rotationParamName = "angularRelativeSpeed";
        private ParamRef rotationParam;
        [SerializeField] private float maxRotationSpeed = 200f;
        [SerializeField, Range(0f, 1f)] private float rotationTreshold = 0.05f;
        public bool IsEmittingRotationSound { get; private set; }
        public float CurrentRelativeRotation { get; private set; }
        
        private Rigidbody2D rigidBody;
        
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            positionParam = new ParamRef { Name = positionParamName };
            rotationParam = new ParamRef { Name = rotationParamName };
        }
        
        private void FixedUpdate()
        {
            SetPositionAmount();
            SetRotationAmount();
            CheckPositionSoundState();
            CheckRotationSoundState();
        }

        private void SetPositionAmount()
        {
            CurrentRelativePosition = (Mathf.Abs(rigidBody.velocity.magnitude) * Time.fixedDeltaTime) / (maxPositionSpeed * Time.fixedDeltaTime);
            positionParam.Value = CurrentRelativePosition;
            audioEmitter.SetParameter(positionSoundInstance, positionParam);
        }
        
        private void SetRotationAmount()
        {    
            CurrentRelativeRotation = (Mathf.Abs(rigidBody.angularVelocity) * Time.fixedDeltaTime) / (maxRotationSpeed * Time.fixedDeltaTime);
            rotationParam.Value = CurrentRelativeRotation;
            audioEmitter.SetParameter(rotationSoundInstance, rotationParam);
        }
         
        private void CheckPositionSoundState()
        {
            if (!emitSoundWhenMove) return;
            
            if (IsEmittingPositionSound)
            {
                if (CurrentRelativePosition < positionTreshold)
                {
                    StopPositionSound();
                }
            }
            else
            {
                if (CurrentRelativePosition >= positionTreshold)
                {
                    audioEmitter.PlaySound(positionSoundEvent, positionParam, out positionSoundInstance);
                    IsEmittingPositionSound = true;
                }
            }
        }
        
        private void CheckRotationSoundState()
        {   
            if (!emitSoundWhenRotate) return;
            
            if (IsEmittingRotationSound)
            {
                if (CurrentRelativeRotation < rotationTreshold)
                {
                    StopRotationSound();
                }
            }
            else
            {
                if (CurrentRelativeRotation >= rotationTreshold)
                {
                    audioEmitter.PlaySound(rotationSoundEvent, rotationParam, out rotationSoundInstance);
                    IsEmittingRotationSound = true;
                }
            }
        }

        private void StopPositionSound()
        {
            audioEmitter.StopSound(positionSoundInstance);
            IsEmittingPositionSound = false;
        }
        
        private void StopRotationSound()
        {
            audioEmitter.StopSound(rotationSoundInstance);
            IsEmittingRotationSound = false;
        }
    }
}
