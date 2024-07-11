using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace ToyBox.Launcher
{


    public class ToySpawner : MonoBehaviour
    {
        const string LOAD_VISUAL_ANIMATION = "LoadToySpawnerVisual";
        const string SPAWN_TOY_ANIMATION = "SpawnToy";
        const string TOY_HOVERING_ANIMATION = "ToyHoverAndSpin";
        const string CLOSE_VISUAL_ANIMATION = "CloseToySpawnerVisual";
        [SerializeField] ToySpawnerData toySpawnerData;
        [SerializeField] Animation animation;
        [SerializeField] Transform visual;
        [SerializeField] Transform toyParent;

        public UnityEvent AnimationComplete = new UnityEvent();
        public UnityEvent HideComplete = new UnityEvent();

        LauncherToy spawnedToy;

        bool isSpawning = false;

        private void Start()
        {
            animation.Play(LOAD_VISUAL_ANIMATION);
        }

        [Button]
        public void SpawnToy()
        {
            if (spawnedToy != null) return;
            if (isSpawning) return;

            AnimationComplete.Invoke();

            isSpawning = true;

            animation.Play(SPAWN_TOY_ANIMATION);
            spawnedToy = InstantiateToy();
            spawnedToy.toySpawner = this;
            //spawnedToy.OnPickup.AddListener(PickupToy);
        }
        LauncherToy InstantiateToy()
        {
            return Instantiate(toySpawnerData.GetRandomToy(), toyParent);
        }

        void PickupToy()
        {
            animation.Play(CLOSE_VISUAL_ANIMATION);
            spawnedToy = null;
        }



        public void FinishAnimation_Event()
        {
            animation.Play(TOY_HOVERING_ANIMATION);
            isSpawning = false;
        }

        public void ShakeVisual_Event()
        {
            visual.DOShakePosition(1f, 0.01f, 10, 90, false, true);
        }

        public void HideSelf()
        {
            //tween to scale 0 and keep the object active
            visual.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                HideComplete.Invoke();
            });
        }
    }

}