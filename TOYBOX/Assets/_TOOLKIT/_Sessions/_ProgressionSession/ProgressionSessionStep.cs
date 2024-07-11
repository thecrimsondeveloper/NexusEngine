using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Sessions
{
    [System.Serializable]
    public class Step : MonoBehaviour, ICompletable
    {
        [SerializeField] string name;
        [SerializeField] internal MonoBehaviour completable;

        [ShowInInspector] public bool IsComplete { get; set; }
        [SerializeField] Transform container;

        public ICompletable Completable => completable as ICompletable;
        public UnityEvent OnComplete { get; set; } = new UnityEvent();
        public UnityEvent OnReset { get; set; } = new UnityEvent();

        private void Start()
        {
            if (completable is ICompletable currCompletable)
            {
                currCompletable.OnComplete.AddListener(CompleteStep);
                currCompletable.OnReset.AddListener(ResetStep);
            }

            if (container == null)
            {
                container = transform;
            }
        }



        private void OnValidate()
        {
            if (completable == null)
            {

            }
            else if (completable is ICompletable completableInts)
            {
                //do nothing
            }
            else
            {
                Debug.LogError($"Completable must implement ICompletable");
                completable = null;
            }
        }

        void CompleteStep()
        {
            (this as ICompletable).Complete();
        }

        void ResetStep()
        {
            (this as ICompletable).Reset();
        }



        public void ActivateContainer(bool enable)
        {
            container.gameObject.SetActive(enable);
        }

        public void Internal_OnComplete()
        {

        }

        public UniTask Introduce()
        {
            return Internal_OnIntroduce();
        }

        protected UniTask Internal_OnIntroduce()
        {
            return UniTask.CompletedTask;
        }

        public async UniTask Activate()
        {
            await (completable as ICompletable).Activate();
        }

        public async UniTask Deactivate()
        {
            await (completable as ICompletable).Deactivate();
        }

        public void Internal_OnReset()
        {

        }

        public string Name => name;

        public bool IsCompletable => Completable is ICompletable;
    }
}
