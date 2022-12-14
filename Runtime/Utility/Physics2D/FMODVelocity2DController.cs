using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FMODVelocity2DController : MonoBehaviour
    {
        [SerializeField] private FMODAudioEmitter audioEmitter;

        [SerializeField] private FMODPhysics2DHelper.DynamicValueSound[] positionEvents;
        [SerializeField] private FMODPhysics2DHelper.DynamicValueSound[] rotationEvents;

        
        private Rigidbody2D rigidBody;
        
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            
            foreach (var position in positionEvents)
            {
                position.Initialize(audioEmitter);
            }
            foreach (var rotation in rotationEvents)
            {
                rotation.Initialize(audioEmitter);
            }
        }
        
        private void FixedUpdate()
        {
            SetPositionAmount();
            SetRotationAmount();
        }

        private void SetPositionAmount()
        {
            foreach (var position in positionEvents)
            {
                position.EvaluateValue(rigidBody.velocity.magnitude);
            }
        }
        
        private void SetRotationAmount()
        {    
            foreach (var rotation in rotationEvents)
            {
                rotation.EvaluateValue(rigidBody.velocity.magnitude);
            }
        }

        public void ForceStopPositionSound()
        {
            foreach (var position in positionEvents)
            {
                position.ForceStop();
            }
        }

        public void ForceStopRotationSound()
        {
            foreach (var rotation in rotationEvents)
            {
                rotation.ForceStop();
            }
        }
    }
}
