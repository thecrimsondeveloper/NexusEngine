using System.Collections;
using System.Collections.Generic;
using ToyBox;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusAttacker : MonoBehaviour
    {
        public NexusWeapon[] weapon;
        public float attackRate = 1f;
        [SerializeField] bool shouldAttack = false;
        [SerializeField] NexusMeleeDetector meleeDetector;
        float lastAttackTime = 0f;

        private void Update()
        {
            if (meleeDetector) shouldAttack = meleeDetector.IsTrue();
            else shouldAttack = false;

            if (shouldAttack && Time.time - lastAttackTime > attackRate)
            {
                Attack();
            }
        }
        public void Attack()
        {
            lastAttackTime = Time.time;
            foreach (var w in weapon)
            {
                w.Attack();
            }
        }
    }
}
