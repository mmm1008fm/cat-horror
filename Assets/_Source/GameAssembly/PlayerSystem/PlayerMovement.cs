using UnityEngine;

namespace GameAssembly.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;

        private Vector2 movement;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (animator == null) animator = GetComponent<Animator>();
        }

        private void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement = movement.normalized;

            // Приоритет по оси с большим значением
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                if (movement.x < 0)
                    animator.Play("WalkLeft");
                else if (movement.x > 0)
                    animator.Play("WalkRight");
            }
            else if (Mathf.Abs(movement.y) > 0)
            {
                if (movement.y < 0)
                    animator.Play("WalkDown");
                else if (movement.y > 0)
                    animator.Play("WalkUp");
            }
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }
}
