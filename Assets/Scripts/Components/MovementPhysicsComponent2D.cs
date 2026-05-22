using UnityEngine;

namespace Components
{
    //Yo! This class is solely for movement stuff. Anything specific like double jumping and what not
    //goes into the concerete class
    
    public class MovementPhysicsComponent2D : MonoBehaviour
    {
        [SerializeField] private float speed = 8f;
        [SerializeField] private float friction = 20f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private Rigidbody2D rb;

        public void MoveVelocity(Vector2 direction)
        {
            if (!rb)
            {
                return;
            }

            Vector2 velocity = rb.linearVelocity;
            float inputX = Mathf.Clamp(direction.x, -1f, 1f);

            if (Mathf.Abs(inputX) > 0.01f)
            {
                velocity.x = inputX * speed;
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0f, friction * Time.deltaTime);
            }

            rb.linearVelocity = velocity;
        }

        public void Jump()
        {
            if (rb == null)
            {
                return;
            }

            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
        }
    }

}