using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

public class Player_ : NetworkBehaviour
{
    private Rigidbody2D rb;
    private NetworkRigidbody2D networkRigidbody2D;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float jumpCooldown = 1f; // Jump cooldown time in seconds
    private float lastJumpTime; // Time of the last jump

    [Networked] private TickTimer cooldown { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        networkRigidbody2D = GetComponent<NetworkRigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsPlayer)
        {
            // Handle input only on the client
            if (GetInput(out NetworkInputData data))
            {
                data.direction2D.Normalize();

                if (data.direction2D.x != 0)
                {
                    // Apply horizontal movement
                    rb.velocity = new Vector2(data.direction2D.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
                    animator.gameObject.GetComponent<SpriteRenderer>().flipX = data.direction2D.x > 0 ? false : true;
                    animator.Play("Running");
                }
                else
                {
                    // Stop horizontal movement
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    animator.Play("Idle");
                }

                // Jump if input indicates so and cooldown is over
                if (data.direction2D.y > 0 && Time.time - lastJumpTime > jumpCooldown)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    lastJumpTime = Time.time;
                }

                // Synchronize position with the server
                // networkRigidbody2D.RBPosition = rb.position;
            }
        }
    }
}
