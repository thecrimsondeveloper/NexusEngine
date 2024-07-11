using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    [Networked] private TickTimer lifeTime { get; set; }

    public void Init()
    {
        lifeTime = TickTimer.CreateFromSeconds(Runner, 5f);
    }

    public override void FixedUpdateNetwork()
    {
        if (lifeTime.Expired(Runner))
        {
            Runner.Despawn(Object);
        }

        transform.position += 5 * transform.forward * Runner.DeltaTime;
    }
}
