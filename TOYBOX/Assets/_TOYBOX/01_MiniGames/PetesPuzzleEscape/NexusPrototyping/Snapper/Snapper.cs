using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToyBox
{
    public class Snapper : MonoBehaviour, IEntity
    {
        [SerializeField] SnapSlot snapPoint;

        public bool HasSnapPoint => snapPoint != null;
        [SerializeField] PointableUnityEventWrapper grabEvents;
        [SerializeField] bool enablePhysicsOnUnselect = true;
        [SerializeField] Rigidbody rb;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            grabEvents.WhenUnselect.AddListener(OnUnselect);
        }

        private void OnUnselect(PointerEvent pointerEvent)
        {
            if (enablePhysicsOnUnselect)
                EnablePhysics(true);


            if (snapPoint) snapPoint.TrySnap();
        }


        public void EnablePhysics(bool enable)
        {
            rb.isKinematic = !enable;
        }

        public void SetSnapSlot(SnapSlot snapPoint)
        {
            this.snapPoint = snapPoint;
        }

        public async UniTask Activate()
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = Vector3.zero;
            await UniTask.Delay(Random.Range(0, 500));
            gameObject.SetActive(true);
            transform.DOScale(localScale, Random.Range(0.3f, 0.5f)).SetEase(Ease.OutBounce);
        }

        public UniTask Deactivate()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
    }
}
