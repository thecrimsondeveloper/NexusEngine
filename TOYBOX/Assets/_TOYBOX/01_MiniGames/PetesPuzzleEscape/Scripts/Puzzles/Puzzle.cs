using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Entity;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace ToyBox.Minigames.EscapeRoom
{
    public class Puzzle : MonoBehaviour, ICompletable
    {
        [SerializeField] protected GameObject clue;
        [SerializeField] public OVRSceneAnchor currentTable;
        [SerializeField] protected VisualEffect solveVFX;

        private bool isComplete = false;

        private UnityEvent onComplete = new UnityEvent();
        private UnityEvent onReset = new UnityEvent();

        public UnityEvent OnPuzzleComplete;

        public bool IsComplete { get => isComplete; set => isComplete = value; }

        UnityEvent ICompletable.OnComplete => onComplete;

        public UnityEvent OnReset => onReset;

        public virtual void OnComplete()
        {
            if (isComplete) return;

            isComplete = true;
            onComplete.Invoke();
            OnPuzzleComplete?.Invoke();
        }

        public void Complete()
        {
            OnComplete();
        }

        protected virtual void DestroyPuzzle(float time)
        {
            Destroy(gameObject, time);
        }

        public void Internal_OnComplete()
        {

        }

        public void Internal_OnReset()
        {
            throw new System.NotImplementedException();
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
