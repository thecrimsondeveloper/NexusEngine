using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Entity
{
    public interface IDestroyable : IHealth
    {
        public void CheckHealth()
        {
            if (health <= 0)
            {
                Destroy();
            }
        }

        public UniTask OnDeath();


        async void Destroy()
        {
            if (this is MonoBehaviour mono)
            {
                await OnDeath();
                GameObject.Destroy(mono.gameObject);
            }

        }





    }
}
