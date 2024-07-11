using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction.Locomotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox.Minigames.BeatEmUp
{
    public class Obstacle : MonoBehaviour, IAttackable
    {
        [SerializeField, FoldoutGroup("Settings")] protected float horizontalSpeed = 1f;
        [SerializeField, FoldoutGroup("Settings")] protected float verticalSpeed = -9.8f;
        [SerializeField, FoldoutGroup("Settings")] protected float lifeTime = 5f;
        [SerializeField, FoldoutGroup("Settings")] protected int maxHealth = 1;
        [SerializeField, FoldoutGroup("Settings")] protected int currentHealth = 1;
        [SerializeField, FoldoutGroup("Dependencies")] protected Animation anim;
        [SerializeField, FoldoutGroup("Dependencies")] protected Rigidbody rb;
        [SerializeField, FoldoutGroup("Dependencies")] protected MeshRenderer meshRenderer;
        [SerializeField, FoldoutGroup("Dependencies")] protected AudioSource audioSource;
        [SerializeField, FoldoutGroup("Debugging")] protected VisualEffect breakVFX;

        [SerializeField, FoldoutGroup("Debugging")] protected bool isGrounded = false;

        protected virtual void Start()
        {
            DestroyObstacle(lifeTime);
        }

        [Button("Break")]
        protected virtual void Break()
        {
            audioSource.Play();
            breakVFX.SendEvent("OnPlay");
            breakVFX.Play();
            meshRenderer.material.DOFloat(1.4f, "_DissolveAmt", 1f).OnComplete(() => DestroyObstacle(.1f));
        }

        public void DestroyObstacle(float delay = 0f)
        {
            Destroy(gameObject, delay);
        }

        private void CheckIsGrounded()
        {
            //raycast to check if the obstacle is grounded
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
        }

        protected virtual void FixedUpdate()
        {
            CheckIsGrounded();
        }

        void IAttackable.OnAttacked(IAttacker attacker)
        {
            if (currentHealth > 0)
            {
                currentHealth -= attacker.strength;
                if (currentHealth <= 0)
                {
                    Break();
                }
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
