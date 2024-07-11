using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Toolkit.Samples;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class ExampleLock : MonoBehaviour, ICompletable
    {
        [SerializeField] Camera rayCastCamera;
        [SerializeField] LockCompletable lockCompletable = new LockCompletable();

        public bool IsComplete { get; set; } = false;

        public UnityEvent OnComplete { get; } = new UnityEvent();

        public UnityEvent OnReset { get; } = new UnityEvent();

        private void Start()
        {
            lockCompletable.OnComplete.AddListener(OnLockComplete);
            lockCompletable.OnReset.AddListener(OnLockReset);
        }

        void OnLockComplete()
        {
            (this as ICompletable).Complete();
        }

        void OnLockReset()
        {
            (this as ICompletable).Reset();
        }

        public void Internal_OnComplete()
        {
            Debug.Log("Internal_OnComplete - ExampleLock");
        }

        // Update is called once per frame


        GameObject lastClickedObject = null;
        bool isUnlocking = false;
        void Update()
        {
            // if (isUnlocking) return;
            //if click
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("click");
                //raycast
                Ray ray = rayCastCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("hit");
                    //if clicked object is this object
                    if (hit.collider.gameObject == gameObject)
                    {
                        if (lockCompletable.CanUnlock(lastClickedObject))
                        {
                            isUnlocking = true;
                            GameObject key = lastClickedObject;
                            key.transform.DOMove(transform.position, 1).OnComplete(() =>
                            {
                                isUnlocking = false;
                                lockCompletable.Unlock(key);
                                key.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
                                {
                                    Destroy(key);
                                });
                            });
                        }
                        lastClickedObject = null;
                        return;
                    }
                    lastClickedObject = hit.collider.gameObject;
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

        public void Internal_OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}
