using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ToyBox.Minigames.BeatEmUp;
using Cysharp.Threading.Tasks;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomUnit : MonoBehaviour, IEntity
    {
        public float health = 10f;
        public PhantomPlayer owner;

        public int strength => throw new System.NotImplementedException();

        public void MoveTo(Vector3 position)
        {
            transform.DOMove(position, 1f);
        }

        public void Initialize(PhantomPlayer owner)
        {
            this.owner = owner;
        }

        public void Damage(float amount)
        {
            health -= amount;
            if (health < 0)
            {
                Die();
            }
        }

        void Die()
        {

        }

        public void Attack(IEntity target)
        {
            if (target is PhantomUnit unit)
            {
                unit.Damage(strength);
            }

            if (target is PhantomBuilding builidng)
            {
                builidng.Damage(strength);
            }
        }


        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }
    }
}
