using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FMODCollision2DController : MonoBehaviour
    {
        [SerializeField] private FMODAudioEmitter audioEmitter;
        [SerializeField] private EventReference collisionSoundEvent;
        [SerializeField] private float collisionTreshold = 5f;
        [SerializeField] private bool isCollisionRelative = true;
        [SerializeField] private string collisionParamName = "collisionForce";
        [SerializeField] private float maxCollisionForce = 40f;
        [SerializeField] private float minTimeBetweenSound = 0.1f;

        private float lastSoundTime;
        
        private Rigidbody2D rigidBody;

        private ParamRef collisionParam;

        private void Awake()
        {
            collisionParam = new ParamRef { Name = collisionParamName };
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (Time.time - lastSoundTime < minTimeBetweenSound)
                return;
            
            float collisionForce = col.relativeVelocity.magnitude;

            if (collisionForce < collisionTreshold)
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
