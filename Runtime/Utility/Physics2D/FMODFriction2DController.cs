using UnityEngine;


namespace MazurkaGameKit.FMODTools
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FMODFriction2DController : MonoBehaviour
    {
         [SerializeField] private FMODAudioEmitter audioEmitter;
         
         [SerializeField] private FMODPhysics2DHelper.DynamicValueSound[] frictionEvents;
         
         private Rigidbody2D rigidBody;
         
         private void Awake()
         {
             rigidBody = GetComponent<Rigidbody2D>();
             
             foreach (var friction in frictionEvents)
             {
                 friction.Initialize(audioEmitter);
             }
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
         }

         private void SetFrictionAmount()
         {
             foreach (var friction in frictionEvents)
             {
                 friction.EvaluateValue(rigidBody.velocity.magnitude);
             }
         }

         private void StopFriction()
         {
             foreach (var friction in frictionEvents)
             {
                 friction.ForceStop();
             }
         }
    }
    
}

