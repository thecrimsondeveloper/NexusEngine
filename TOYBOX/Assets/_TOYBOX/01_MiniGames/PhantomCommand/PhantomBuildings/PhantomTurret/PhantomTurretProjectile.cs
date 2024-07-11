using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ToyBox.Games.PhantomCommand;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomTurretProjectile : MonoBehaviour
    {
        [Title("Projectile Dependencies")]
        [SerializeField, Required] private AudioSource audioSource;
        [SerializeField, Required] private AudioClip hitSound;
        // [SerializeField, Required] private VisualEffect hitVFX;
        public Rigidbody rb;



        [Title("Populated at Runtime")]
        public PhantomTurret turret;



        private void OnCollisionEnter(Collision collision)
        {
            //check whether the object we collided with is a PhantomUnit through the PhantomUnit component
            if (collision.collider.TryGetComponent(out PhantomUnit unit) && unit.owner != turret.owner)
            {
                unit.Damage(turret.Strength);
            }

            Destroy(gameObject);
        }
    }
}
