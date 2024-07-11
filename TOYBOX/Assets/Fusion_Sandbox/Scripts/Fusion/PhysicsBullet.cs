using Fusion;
using UnityEngine;

public class PhysicsBullet : NetworkBehaviour
{
    [Networked] private TickTimer lifeTime { get; set; }

    public void Init(Vector3 forward)
    {
        lifeTime = TickTimer.CreateFromSeconds(Runner, 5f);
        GetComponent<Rigidbody>().velocity = forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (lifeTime.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
    }
}
