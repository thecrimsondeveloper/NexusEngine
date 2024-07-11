using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class
    CosmicTutorialStep : MonoBehaviour, ICompletable
    {
        public GameObject[] controlObjects = null;
        public bool IsComplete { get; set; } = false;
        public bool IsTutorialActive => gameObject.activeInHierarchy;

        public UnityEvent OnComplete { get; } = new UnityEvent();

        public UnityEvent OnReset { get; } = new UnityEvent();

        public void Internal_OnComplete()
        {
        }

        public void Internal_OnReset()
        {
        }

        public UniTask Activate()
        {
            gameObject.SetActive(true);
            //loop through objectsToActivate and set them to active
            foreach (GameObject obj in controlObjects)
            {
                obj.transform.localScale = Vector3.zero;
                obj.SetActive(true);

                obj.transform.DOScale(1, 0.5f);
            }
            return UniTask.CompletedTask;
        }

        public void Hide()
        {

            //loop through objectsToActivate and set them to inactive
            foreach (GameObject obj in controlObjects)
            {
                obj.SetActive(false);
            }
            gameObject.SetActive(false);
        }

        public void DetachObjects()
        {
            foreach (GameObject obj in controlObjects)
            {
                obj.transform.parent = transform.parent;
            }

            //clear the list
            controlObjects = new GameObject[0];
        }

        public async UniTask Deactivate()
        {
            //loop through objectsToActivate and set them to inactive
            foreach (GameObject obj in controlObjects)
            {
                obj.transform.DOScale(0, 0.5f).OnComplete(() => obj.SetActive(false));
            }
            await UniTask.Delay(750);
            gameObject.SetActive(false);
        }

        public UnityEvent OnCompleteTutorialStep = new UnityEvent();

        [Button("Complete Tutorial Step")]
        protected void CompleteTutorialStep()
        {
            OnCompleteTutorialStep.Invoke();
            (this as ICompletable).Complete();
        }

    }
}
