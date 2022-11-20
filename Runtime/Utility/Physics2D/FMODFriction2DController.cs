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
    public class FMODFriction2DController : MonoBehaviour
    {
        [SerializeField] private FMODAudioEmitter audioEmitter;
        [SerializeField] private EventReference frictionSoundEvent;
         private EventInstance frictionSoundInstance;
         [SerializeField] private string frictionParamName = "frictionAmount";
         private ParamRef frictionParam;
         private Rigidbody2D rigidBody;

         [SerializeField] private float maxFrictionSpeed = 3f;
         [SerializeField, Range(0f, 1f)] private float frictionTreshold = 0.05f;

         public bool IsEmittingFriction { get; private set; }
         public float CurrentRelativeFriction { get; private set; }
         private void Awake()
         {
             rigidBody = GetComponent<Rigidbody2D>();
             frictionParam = new ParamRef { Name = frictionParamName };
         }

         public bool IsOnGround { get; private set; }
         
         private void OnCollisionEnter2D(Collision2D col)
         {
             if (IsOnGround) return;
             else IsOnGround = true;
         }

         private void OnCollisionExit2D(Collision2D other)
         {
             if (!IsOnGround) return;
             else IsOnGround = false;

             StopFriction();
         }

         private void FixedUpdate()
         {
             if (!IsOnGround) return;

             SetFrictionAmount();
             CheckSoundState();
         }

         private void SetFrictionAmount()
         {
             CurrentRelativeFriction = (Mathf.Abs(rigidBody.velocity.magnitude) * Time.fixedDeltaTime) / (maxFrictionSpeed * Time.fixedDeltaTime);

             frictionParam.Value = CurrentRelativeFriction;
             audioEmitter.SetParameter(frictionSoundInstance, frictionParam);
         }
         
         private void CheckSoundState()
         {
             if (IsEmittingFriction)
             {
                 if (CurrentRelativeFriction < frictionTreshold)
                 {
                     StopFriction();
                 }
             }
             else
             {
                 if (CurrentRelativeFriction >= frictionTreshold)
                 {
                     audioEmitter.PlaySound(frictionSoundEvent, frictionParam, out frictionSoundInstance);
                     IsEmittingFriction = true;
                 }
             }
         }

         private void StopFriction()
         {
             audioEmitter.StopSound(frictionSoundInstance);
             IsEmittingFriction = false;
         }
    }
    
}

