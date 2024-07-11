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
    public class SnapSlot : MonoBehaviour, ICompletable
    {
        [SerializeField] Snapper snapper;
        [SerializeField] float snapDistance = 0.1f;
        [SerializeField] bool canUnsnap = true;
        public enum SnapState
        {
            Unsnapped,
            Snapping,
            Snapped
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, snapDistance);
        }

        public SnapState snapState = SnapState.Unsnapped;

        public bool IsComplete { get; set; } = false;


        public UnityEvent<SnapSlot> OnSnap { get; set; } = new UnityEvent<SnapSlot>();

        public UnityEvent OnComplete { get; set; } = new UnityEvent();

        public UnityEvent OnReset { get; set; } = new UnityEvent();
        [Button]
        public void TrySnap()
        {


            float distance = Vector3.Distance(transform.position, snapper.transform.position);



            if (distance < snapDistance && snapState == SnapState.Unsnapped)
            {
                Snap();
            }
            else if (snapState == SnapState.Snapped && canUnsnap)
            {
                Unsnap();
            }
        }

        public void Snap()
        {
            if (snapState == SnapState.Snapping)
            {
                return;
            }
            snapState = SnapState.Snapping;

            snapper.EnablePhysics(false);

            snapper.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
            {
                snapState = SnapState.Snapped;
                (this as ICompletable).Complete();
                snapper.EnablePhysics(false);
            });
        }

        public void Unsnap()
        {
            snapState = SnapState.Unsnapped;

        }

        public void Internal_OnComplete()
        {
            OnSnap.Invoke(this);
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

        public void SetSnapper(Snapper snapper)
        {
            this.snapper = snapper;
        }
    }
}
