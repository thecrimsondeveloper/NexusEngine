using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ToyBox
{
    public class BasicMole : Mole
    {
        protected override void OnInitialize()
        {
            animation.Play(ANIM_SPAWN_NAME);
        }
        protected override void OnHit()
        {
            // animation.Play(ANIM_HIT_NAME);
        }

        protected override void OnUpdate()
        {
            if (bodyRigidbody.angularVelocity.magnitude < 0.01f)
            {
                bodyRigidbody.AddTorque(Random.insideUnitSphere * Time.deltaTime * 0.01f, ForceMode.Impulse);
            }
        }
    }
}
