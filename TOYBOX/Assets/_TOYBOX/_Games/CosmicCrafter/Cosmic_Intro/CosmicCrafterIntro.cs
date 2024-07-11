using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using ToyBox.Extras;
using Oculus.Interaction;
using TMPro;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using ToyBox.Minigames.CosmicCrafter;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicCrafterIntro : MonoBehaviour
    {
        public bool autoComplete = false;

        [Title("Intro Visuals")]
        [SerializeField] Transform elementParent;
        [SerializeField] CosmicBlackHole blackHole;
        [SerializeField] Vector3 blackHoleStartLocalScale = Vector3.one;


        [Title("Cosmic Crafter Tools Intro")]
        [SerializeField] TransformGrabbable moveAndRotate;
        [SerializeField] GameObject moveAndRotateDirections;
        [SerializeField] bool hasMoveAndRotateBeenGrabbed = false;
        [SerializeField] TransformGrabbable move;
        [SerializeField] GameObject moveDirections;
        [SerializeField] bool hasMoveBeenGrabbed = false;
        [SerializeField] TransformGrabbable rotate;
        [SerializeField] GameObject rotateDirections;
        [SerializeField] bool hasRotateBeenGrabbed = false;





        [Title("Events")]
        public UnityEvent OnCompleteIntro = new UnityEvent();

        void Start()
        {

            moveAndRotate.OnGrabbed.AddListener(OnGrabMoveAndRotate);
            move.OnGrabbed.AddListener(OnGrabMove);
            rotate.OnGrabbed.AddListener(OnGrabRotate);

            moveAndRotate.OnReleased.AddListener(OnReleaseMoveAndRotate);
            move.OnReleased.AddListener(OnReleaseMove);
            rotate.OnReleased.AddListener(OnReleaseRotate);
            blackHoleStartLocalScale = blackHole.transform.localScale;





            if (autoComplete)
            {
                Complete();
            }
        }

        void OnGrabMoveAndRotate()
        {
            hasMoveAndRotateBeenGrabbed = true;
            moveAndRotateDirections.SetActive(false);

        }

        void OnGrabMove()
        {
            hasMoveBeenGrabbed = true;
            moveDirections.SetActive(false);
        }

        void OnGrabRotate()
        {
            hasRotateBeenGrabbed = true;
            rotateDirections.SetActive(false);
        }

        void CheckIfCompletedIntro()
        {
            if (hasMoveAndRotateBeenGrabbed && hasMoveBeenGrabbed && hasRotateBeenGrabbed)
            {
                Complete();
            }
        }

        void OnReleaseMoveAndRotate()
        {
            CheckIfCompletedIntro();
        }

        void OnReleaseMove()
        {
            CheckIfCompletedIntro();
        }

        void OnReleaseRotate()
        {
            CheckIfCompletedIntro();
        }

        [Button]
        void Complete()
        {

            OnCompleteIntro?.Invoke();

            blackHole.transform.SetParent(elementParent);
            blackHole.transform.DOScale(Vector3.zero, 1).OnComplete(() =>
            {
                blackHole.transform.position = Vector3.zero;
                blackHole.transform.localScale = blackHoleStartLocalScale;
            });
        }

    }
}
