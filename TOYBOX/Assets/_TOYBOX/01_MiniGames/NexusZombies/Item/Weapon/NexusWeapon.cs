using Sirenix.OdinInspector;
using Toolkit.Entity;
using ToyBox;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public abstract class NexusWeapon : NexusItem
    {
        [SerializeField, Range(0, 50)] protected float attackRate = 0.5f;
        public float damage = 10f;
        public float headShotMultiplier = 2f;


        [SerializeField] bool manualAttack = false;

        public float headshotDamage => damage * headShotMultiplier;
        float lastFireTime = 0f;

        protected override void OnUse(NexusPlayer player)
        {
            if (manualAttack)
            {

            }
            else
            {
                Attack();
            }
        }

        [Button, HideInEditorMode]
        public void Attack()
        {
            if (Time.time - lastFireTime <= 1 / attackRate) return;
            if (onUseAnimation.isPlaying) return;

            lastFireTime = Time.time;
            OnAttack();
        }

        protected abstract void OnAttack();

        public virtual void OnAttackableAttacked(IAttackable attackable)
        {

        }
    }
}