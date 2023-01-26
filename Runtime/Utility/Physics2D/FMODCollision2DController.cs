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

        [SerializeField] private FMODPhysics2DHelper.Collision2DSound[] collisionEvents;
        
        private ParamRef collisionParam;

        private void Awake()
        {
            foreach (var collision in collisionEvents)
            {
                collision.Initialize(audioEmitter);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            foreach (var collision in collisionEvents)
            {
                collision.EvaluateCollision(col.relativeVelocity.magnitude);
            }
        }

        private void OnValidate()
        {
            if (audioEmitter == null)
            {
                audioEmitter = GetComponentInChildren<FMODAudioEmitter>();
            }
        }
    }
}
