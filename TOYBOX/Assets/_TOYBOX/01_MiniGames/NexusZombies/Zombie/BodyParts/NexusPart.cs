using System.Collections;
using System.Collections.Generic;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class NexusPart : MonoBehaviour, IAttackable
    {
        public NexusCharacter character;
        public UnityEvent<NexusWeapon> WhenAttacked { get; set; } = new UnityEvent<NexusWeapon>();
        public UnityEvent<NexusPart, NexusWeapon> OnPartAttacked { get; set; } = new();

        public void InitializePart(NexusCharacter character)
        {
            this.character = character;
            OnPartInitialized(character);
        }

        protected virtual void OnPartInitialized(NexusCharacter character)
        {
            Debug.Log("Part initialized!");
        }

        public void OnAttacked(NexusWeapon weapon)
        {
            Debug.Log("Part attacked!");
            OnPartAttacked.Invoke(this, weapon);
        }
    }
}
