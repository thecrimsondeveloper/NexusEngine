using Cysharp.Threading.Tasks;
using Toolkit;
using Toolkit.Sequences;
using UnityEngine;

public class PlayerMovementHandler : MonoSequence, ITickable
{
    public float gravity = -9.81f;
    public float drag = 0.1f;
    public float jumpForce = 5;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 velocity;

    float timeSinceGrounded = 0;

    public int TickRate { get; } = 1;

    public void OnTick()
    {
        if (currentData is Player player)
        {
            if (controller.isGrounded)
            {
                timeSinceGrounded = 0;
            }
            else
            {
                timeSinceGrounded += Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                Jump();
            }
        }
        ApplyGravityAndDrag();
        MovePlayer();


    }

    private void Jump()
    {
        velocity.y = jumpForce;
    }

    private void ApplyGravityAndDrag()
    {
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        velocity.x *= 1 - drag * Time.deltaTime;
        velocity.z *= 1 - drag * Time.deltaTime;
    }

    private void MovePlayer()
    {
        Vector3 move = velocity * Time.deltaTime;
        CollisionFlags flags = controller.Move(move);
        switch (flags)
        {
            case CollisionFlags.Below:
                velocity.y = 0;
                break;
            case CollisionFlags.Above:
                velocity.y = 0;
                break;
            case CollisionFlags.Sides:
                velocity.x = 0;
                velocity.z = 0;
                break;
        }
    }

    protected override async UniTask Finish()
    {
        Debug.Log("PlayerMovementHandler finished.");
        await UniTask.CompletedTask;
    }

    protected override async UniTask WhenLoad()
    {
        await UniTask.CompletedTask;
    }

    protected override async UniTask Unload()
    {
        await UniTask.CompletedTask;
    }

    protected override void OnStart()
    {
        Debug.Log("PlayerMovementHandler started.");
    }

    protected override void AfterLoad()
    {
        Debug.Log("PlayerMovementHandler after load.");
    }

    protected override void OnFinished()
    {
        Debug.Log("PlayerMovementHandler finished.");
    }

    protected override void OnUnload()
    {
        Debug.Log("PlayerMovementHandler unloaded.");
    }


}
