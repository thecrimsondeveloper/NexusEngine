using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


namespace ToyBox.Launcher
{

    public class LauncherToy : MonoBehaviour
    {
        public ToySpawner toySpawner;
        [SerializeField] string sessionDataVarStandin = "LauncherToy";

        [SerializeField] InteractableUnityEventWrapper interactableEvents;
        // [SerializeField] TriggerButton triggerButton;

        [FoldoutGroup("Events")] public UnityEvent<string> OnLaunchEvent;
        [FoldoutGroup("Events")] public UnityEvent OnPickup;
        [FoldoutGroup("Events")] public UnityEvent OnDrop;

        private void Start()
        {
            interactableEvents.WhenUnselect.AddListener(Pickup);
            // triggerButton.OnClick.AddListener(Pickup);
        }

        void Drop()
        {
            OnDrop.Invoke();
        }
        void Pickup()
        {
            OnPickup.Invoke();
            transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
            toySpawner.HideSelf();
        }

        public void OnLaunch()
        {
            Debug.Log("OnLaunch: " + sessionDataVarStandin);
            OnLaunchEvent.Invoke(sessionDataVarStandin);
        }
    }

}