using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicTeleporter : MonoBehaviour
    {
        [SerializeField] private Transform exitPoint;
        [SerializeField] private Transform entryPoint;
        [SerializeField] private AudioClip teleportClip;

        [SerializeField] private AudioClip teleportOutput;
        [SerializeField, Range(0, 25)] private float exitForce = 1f;
        [SerializeField] private int teleportDuration = 3;
        [SerializeField] MeshRenderer ropeMeshRenderer;
        [SerializeField] Animation outputVFX;
        [SerializeField] PointableUnityEventWrapper events;
        [SerializeField] PointableUnityEventWrapper outputGrabbableEvents;
        [SerializeField] float cableDistance = 1.0f;
        bool isTeleporterBeingHeld = false;
        bool isOutputBeingHeld = false;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, cableDistance);
        }

        private void Start()
        {
            events.WhenSelect.AddListener(WhenTeleporterGrabbed);
            events.WhenRelease.AddListener(WhenTeleporterReleased);

            outputGrabbableEvents.WhenSelect.AddListener(WhenOutputGrabbed);
            outputGrabbableEvents.WhenRelease.AddListener(WhenOutputReleased);
        }


        private void Update()
        {

            if (isOutputBeingHeld)
            {

                Vector3 directionTowardGhost = outputGrabbableEvents.transform.position - transform.position;
                Vector3 clampedDirection = Vector3.ClampMagnitude(directionTowardGhost, cableDistance);
                Debug.DrawRay(outputGrabbableEvents.transform.position, clampedDirection, Color.green);

                //set the target position to the grabbableEvents position
                Vector3 targetPosition = transform.position + clampedDirection;
                exitPoint.position = Vector3.Lerp(exitPoint.position, targetPosition, Time.deltaTime * 100);
                exitPoint.rotation = outputGrabbableEvents.transform.rotation;
            }
        }

        [Button]
        private void WhenTeleporterReleased(PointerEvent evt)
        {
            isTeleporterBeingHeld = false;
        }

        [Button]
        private void WhenTeleporterGrabbed(PointerEvent evt)
        {
            if ((Session.CurrentSession as RoundBasedSession).TryGetCurrentRound(out Round currentRound))
            {
                transform.parent = currentRound.ContentParent;
            }
            isTeleporterBeingHeld = true;
        }

        [Button]
        private void WhenOutputReleased(PointerEvent evt)
        {
            outputGrabbableEvents.transform.position = exitPoint.position;
            outputGrabbableEvents.transform.rotation = exitPoint.rotation;

            isOutputBeingHeld = false;
        }

        [Button]

        private void WhenOutputGrabbed(PointerEvent evt)
        {
            isOutputBeingHeld = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (isTeleporterBeingHeld)
            {
                return;
            }


            if (other.TryGetComponent(out CosmicStar star))
            {
                Teleport(star);
            }
        }

        private async void Teleport(CosmicStar star)
        {
            //if the audio is not null, play the audio
            if (teleportClip)
            {
                AudioSource.PlayClipAtPoint(teleportClip, transform.position);
            }

            star.FreezeVelocity();
            star.SetKinematic(true);
            star.transform.DOMove(entryPoint.position, 0.2f);
            await star.AsyncShrink(0.2f);
            if (star == null) return;
            star.gameObject.SetActive(false);

            ropeMeshRenderer.material.DOFloat(0.5f, "_Progress", teleportDuration);
            await UniTask.Delay((int)(teleportDuration * 1000f));
            if (star == null) return;


            ropeMeshRenderer.material.SetFloat("_Progress", 0);
            star.transform.position = exitPoint.position;
            star.gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(teleportOutput, transform.position);



            await star.AsyncGrow(0.5f);
            if (star == null) return;

            star.SetKinematic(false);
            outputVFX.Play();
            //add force to cosmic star
            star.Push(exitPoint.forward, exitForce);
        }
    }
}
