using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.Events;
using Toolkit.Entity;
using Cysharp.Threading.Tasks;

namespace ToyBox.Minigames.EscapeRoom
{

    public class RingBase : MonoBehaviour, ICompletable
    {

        [System.Serializable]
        class RingSlot
        {
            public Vector3 snapPoint;
            public Ring ring;

            public Vector3 GetTransformPoint(Transform transform)
            {
                return transform.TransformPoint(snapPoint);
            }
        }

        [SerializeField] RingSlot[] ringSlots = new RingSlot[3];

        [SerializeField] bool isWinningRing = false;

        [SerializeField] Transform dropPoint;

        Transform currentSnapPoint = null;

        public bool IsComplete { get; set; } = false;

        public UnityEvent OnComplete { get; set; } = new UnityEvent();

        public UnityEvent OnReset { get; set; } = new UnityEvent();

        bool placingRing = false;


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(dropPoint.position, 0.02f);

            for (int i = 0; i < ringSlots.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.position + ringSlots[i].snapPoint, 0.02f);

                //draw a label for the snap point
                //UnityEditor.Handles.Label(transform.position + ringSlots[i].snapPoint, "" + i);
            }
        }

        [Button]
        public async void PlaceRing(Ring ring)
        {
            if (placingRing)
            {
                return;
            }

            placingRing = true;
            ring.transform.DOMove(dropPoint.position, .25f);
            ring.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0f, 0f), 0.5f);

            await UniTask.Delay(500);



            // Find the first open slot
            RingSlot openSlot = FindOpenSlot();

            bool shouldPlaceRing = openSlot.ring == null || openSlot.ring.size > ring.size;
            if (shouldPlaceRing)
            {
                //set the ring to be child of the ring base
                ring.transform.SetParent(transform);


                // Place the ring in the open slot
                ring.transform.DOLocalMove(openSlot.snapPoint, .5f).OnComplete(() =>
                {

                    //if the open slot is the top slot then complete the puzzle
                    if (openSlot.snapPoint == ringSlots[2].snapPoint)
                    {
                        (this as ICompletable).Complete();
                    }
                    placingRing = false;
                });
                openSlot.ring = ring;
            }
            else
            {
                placingRing = false;
            }
        }

        private RingSlot FindOpenSlot()
        {
            foreach (RingSlot slot in ringSlots)
            {
                if (slot.ring == null)
                {
                    return slot;
                }
            }
            return null;
        }

        public void RemoveRing(Ring ring)
        {
            foreach (RingSlot slot in ringSlots)
            {
                if (slot.ring == ring)
                {
                    // slot.ring = null;
                }
            }
        }

        public void Internal_OnComplete()
        {
            throw new System.NotImplementedException();
        }

        public void Internal_OnReset()
        {
            throw new System.NotImplementedException();
        }

        public UniTask Activate()
        {
            throw new System.NotImplementedException();
        }

        public UniTask Deactivate()
        {
            throw new System.NotImplementedException();
        }
    }

}
