using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Extras
{
    public class TransformGrabbable : MonoBehaviour
    {


        [Title("Dependencies")]
        public PointableUnityEventWrapper grabbableEvents;
        [SerializeField, HideInEditorMode, BoxGroup("Debug")] protected Transform startingParent = null;
        [SerializeField, HideInEditorMode, BoxGroup("Debug")] protected Pose startingPose = Pose.identity;
        [SerializeField, HideInEditorMode, BoxGroup("Debug")] protected Pose relativePoseWhenGrabFromParent = Pose.identity;
        [SerializeField, HideInEditorMode, BoxGroup("Debug")] protected Vector3 localScaleWhenGrabbed = Vector3.one;
        [SerializeField, HideInEditorMode, BoxGroup("Debug")] protected Vector3 lossyScaleWhenGrabbed = Vector3.one;
        [SerializeField, HideInEditorMode, BoxGroup("Debug")] protected bool isGrabbed = false;

        public UnityEvent OnGrabbed = new UnityEvent();
        public UnityEvent OnReleased = new UnityEvent();
        // Start is called before the first frame update

        private void OnValidate()
        {
            if (grabbableEvents == null)
            {
                grabbableEvents = GetComponent<PointableUnityEventWrapper>();
            }

            if (grabbableEvents == null)
            {
                grabbableEvents = gameObject.GetComponentInChildren<PointableUnityEventWrapper>();
            }
        }

        protected virtual void OnDrawGizmos()
        {

        }
        private void Start()
        {
            startingParent = transform.parent;


            startingPose = new Pose(transform.localPosition, transform.localRotation);

            // Debug.Log("Starting Parent: " + startingParent);

            grabbableEvents.WhenSelect.AddListener(OnGrab);
            grabbableEvents.WhenUnselect.AddListener(OnRelease);
        }

        private void Update()
        {
            if (isGrabbed)
            {
                OnUpdateParent(startingParent);
            }
        }

        protected virtual void OnUpdateParent(Transform parent)
        {

        }

        [Button("OnGrab")]
        public virtual void OnGrab(PointerEvent evt)
        {
            localScaleWhenGrabbed = transform.localScale;


            Debug.Log("OnGrab");
            Vector3 grabbingPosition = transform.position - startingParent.position;

            relativePoseWhenGrabFromParent = new Pose(grabbingPosition, transform.localRotation);
            localScaleWhenGrabbed = transform.localScale;




            isGrabbed = true;

            // transform.SetParent(null);

            transform.localScale = localScaleWhenGrabbed;
            OnGrabbed.Invoke();
        }

        [Button("OnRelease")]
        public virtual void OnRelease(PointerEvent evt)
        {
            Debug.Log("OnRelease");
            isGrabbed = false;
            ResetGrabbable();
            OnReleased.Invoke();
        }


        public virtual void ResetGrabbable()
        {
            ResetParent();
            SetLocalPose(startingPose);
        }

        public void ResetParent()
        {
            if (startingParent != null)
            {
                transform.SetParent(startingParent);
                transform.localScale = localScaleWhenGrabbed;
            }
        }

        public void SetLocalPose(Pose pose)
        {
            transform.localPosition = pose.position;
            transform.localRotation = pose.rotation;
        }


        public void SetParentOrientation(AlignmentAxis axis, Vector3 direction)
        {
            Debug.Log("Setting Orientation: " + direction + " Axis: " + axis);
            switch (axis)
            {
                case AlignmentAxis.RIGHT:
                    startingParent.right = direction;
                    break;
                case AlignmentAxis.UP:
                    startingParent.up = direction;
                    break;
                case AlignmentAxis.FORWARD:

                    break;
            }
        }

        public void LerpParentOrientation(AlignmentAxis axis, Vector3 direction, float lerpSpeed)
        {
            Debug.DrawRay(transform.position, direction, Color.red);
            Debug.Log("Lerping Parent Orientation: " + direction);
            switch (axis)
            {
                case AlignmentAxis.RIGHT:
                    startingParent.right = Vector3.Lerp(startingParent.right, direction, lerpSpeed * Time.deltaTime);
                    break;
                case AlignmentAxis.UP:
                    startingParent.up = Vector3.Lerp(startingParent.up, direction, lerpSpeed * Time.deltaTime);
                    break;
                case AlignmentAxis.FORWARD:
                    startingParent.forward = Vector3.Lerp(startingParent.forward, direction, lerpSpeed * Time.deltaTime);
                    break;
            }
        }



        public T AddComponentToParent<T>() where T : Component
        {
            if (startingParent.GetComponent<T>() == null)
            {
                return startingParent.gameObject.AddComponent<T>();
            }
            return startingParent.GetComponent<T>();
        }

        public bool RemoveComponentFromParent<T>() where T : Component
        {
            T comp = startingParent.GetComponent<T>();
            if (comp != null)
            {
                Destroy(comp);
            }

            comp = startingParent.GetComponent<T>();
            return comp == null;
        }
    }

    public enum AlignmentAxis
    {
        RIGHT,
        UP,
        FORWARD
    }
}
