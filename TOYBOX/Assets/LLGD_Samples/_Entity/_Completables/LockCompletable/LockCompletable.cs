using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Samples
{
    [System.Serializable]
    public class LockCompletable : ICompletable
    {
        enum LockType { Key, MultiKey }
        [SerializeField] LockType lockType = LockType.Key;
        [SerializeField] List<GameObject> keys = new List<GameObject>();

        public bool IsComplete { get; set; } = false;

        public UnityEvent OnComplete { get; } = new UnityEvent();

        public UnityEvent OnReset { get; } = new UnityEvent();

        public void Internal_OnComplete()
        {

        }

        public bool CanUnlock(GameObject key)
        {
            return keys.Contains(key);
        }

        /* 
        <summary> Returns true if the key was a match in the possible key list. </summary>
        */
        public bool Unlock(GameObject key)
        {

            bool containsKey = keys.Contains(key);
            if (containsKey == false) return false;

            if (lockType == LockType.Key)
            {
                (this as ICompletable).Complete();
            }
            else if (lockType == LockType.MultiKey)
            {
                keys.Remove(key);
                if (keys.Count == 0)
                {
                    (this as ICompletable).Complete();
                }
            }

            return true;
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
