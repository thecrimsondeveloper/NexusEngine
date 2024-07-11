using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.NexusEngine;
using UnityEngine.Events;

namespace Toolkit.Entity
{
    public interface IAttackable
    {
        public UnityEvent<NexusWeapon> WhenAttacked { get; set; }
        public void Attack(NexusWeapon weapon)
        {
            Debug.Log("I'm attacked!");
            weapon.OnAttackableAttacked(this);
            WhenAttacked.Invoke(weapon);

            OnAttacked(weapon);
        }

        void OnAttacked(NexusWeapon weapon);
    }
}
