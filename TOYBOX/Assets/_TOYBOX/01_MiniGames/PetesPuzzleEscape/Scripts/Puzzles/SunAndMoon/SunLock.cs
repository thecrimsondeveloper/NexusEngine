using System.Collections;
using System.Collections.Generic;
using Toolkit.Samples;
using ToyBox.Minigames.EscapeRoom;
using UnityEngine;
using UnityEngine.Events;
using Toolkit.Entity;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace ToyBox
{
    public class SunLock : MonoBehaviour, ICompletable
    {
        [SerializeField] private LockCompletable lockCompletable = new LockCompletable();

        [ShowInInspector] public bool IsComplete { get; set; } = false;

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

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out SunKey sunkey) && !IsComplete)
            {
                lockCompletable.Unlock(other.gameObject);
                Pose pose = new Pose(transform.position - new Vector3(0f, 0f, 0.01f), other.transform.rotation);
                other.gameObject.GetComponent<SunKey>().LockInPlace(pose);
            }
        }

        void OnLockReset()
        {
            (this as ICompletable).Reset();
        }

        public void Internal_OnComplete()
        {
        }

        public void Internal_OnReset()
        {
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
