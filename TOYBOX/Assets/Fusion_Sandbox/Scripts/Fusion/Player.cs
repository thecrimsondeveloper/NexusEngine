using UnityEngine;
using Fusion;
using UnityEngine.AI;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private PhysicsBullet _physicsBulletPrefab;
    [SerializeField] private ChangeDetector changeDetector;
    [SerializeField] private Material material;
    private Vector3 _forward = Vector3.forward;

    [Networked] private TickTimer cooldown { get; set; }

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();

    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            characterController.Move(10 * data.direction * Runner.DeltaTime);
        }

        if (data.direction.sqrMagnitude > 0)
        {
            _forward = data.direction;
        }

        if (HasStateAuthority && cooldown.ExpiredOrNotRunning(Runner))
        {
            //instantiate bullet
            if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
            {
                Runner.Spawn(_bulletPrefab, transform.position + _forward, Quaternion.LookRotation(_forward), Object.InputAuthority,
                (Runner, O) =>
                {
                    Bullet bullet = O.GetComponent<Bullet>();
                    bullet.Init();
                });
            }

            //instantiate bullet
            else if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON1))
            {
                Runner.Spawn(_physicsBulletPrefab, transform.position + _forward, Quaternion.LookRotation(_forward), Object.InputAuthority,
                (Runner, O) =>
                {
                    PhysicsBullet bullet = O.GetComponent<PhysicsBullet>();
                    bullet.Init(10 * _forward);
                });
            }
        }


    }


}
