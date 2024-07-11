using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

namespace Toolkit.NexusEngine
{
    public class NexusCharacter : MonoBehaviour, ICharacter
    {

        public static Transform rightHand;
        public static Transform leftHand;
        [SerializeField] float _health = 100;
        public float health
        {
            get => _health;
            set => _health = value;
        }

        [SerializeField] NexusPart[] nexusParts;
        public UnityEvent<float> OnHealthChanged { get; } = new UnityEvent<float>();
        public UnityEvent<float> OnScoreEvent { get; set; } = new UnityEvent<float>();

        protected virtual void Start()
        {
            nexusParts = GetComponentsInChildren<NexusPart>();

            foreach (var part in nexusParts)
            {
                part.OnPartAttacked.AddListener(OnPartAttacked);
                part.InitializePart(this);
            }
        }

        public void OnPartAttacked(NexusPart part, NexusWeapon weapon)
        {
            Debug.Log("Part attacked Event Called!");
            float damage = part is NexusHead ? weapon.headshotDamage : weapon.damage;

            //add score based
            OnScoreEvent.Invoke(1);
            (this as IHealth).ChangeHealth(-damage);
        }

        public void OnChangeHealth(float damage)
        {
            Debug.Log("Health changed now checking!");
            (this as IDestroyable).CheckHealth();
        }

        [Button]
        public void FindAllBodyParts()
        {
            nexusParts = GetComponentsInChildren<NexusPart>();
        }

        public async UniTask OnDeath()
        {
            OnScoreEvent.Invoke(10);
            bool isShrinking = false;
            transform.DOScale(Vector3.zero, 2f).OnStart(() => isShrinking = true)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => isShrinking = false);
            await UniTask.WaitUntil(() => !isShrinking);
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
