using System.Collections;
using System.Collections.Generic;
using Toolkit.Entity;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class NexusMeleeDetector : MonoBehaviour
    {
        List<IAttackable> attackables = new List<IAttackable>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IAttackable attackable))
            {
                if (!attackables.Contains(attackable))
                {
                    attackables.Add(attackable);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IAttackable attackable))
            {
                if (attackables.Contains(attackable))
                {
                    attackables.Remove(attackable);
                }
            }
        }

        public bool IsTrue()
        {
            return attackables.Count > 0;
        }
    }
}
