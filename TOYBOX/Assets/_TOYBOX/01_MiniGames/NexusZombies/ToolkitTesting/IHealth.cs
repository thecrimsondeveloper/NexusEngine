using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Entity
{
    public interface IHealth
    {
        float health { get; set; }
        public UnityEvent<float> OnHealthChanged { get; }

        public virtual void ChangeHealth(float damage)
        {
            Debug.Log("Health changed!");
            health += damage;
            OnHealthChanged.Invoke(damage);
            OnChangeHealth(damage);
        }

        public void OnChangeHealth(float damage);

    }
}
