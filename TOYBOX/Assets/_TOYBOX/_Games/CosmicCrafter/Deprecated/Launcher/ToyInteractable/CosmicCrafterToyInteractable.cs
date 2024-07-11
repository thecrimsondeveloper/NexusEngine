using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToyBox.Sessions;
using Toolkit.XR;
using Toolkit.Sessions;
using UnityEngine.VFX;
using Cysharp.Threading.Tasks;

namespace ToyBox.Minigames.CosmicCrafter
{
    public class CosmicCrafterToyInteractable : MonoBehaviour
    {
        [SerializeField] private SessionLauncher sessionLauncher = null;
        [SerializeField] Gradient colorOverHoldTime = null;
        [SerializeField] private float distanceToInteract = 0.05f;
        [SerializeField] float timeToHold = 2f;
        [SerializeField] VisualEffect visualEffect = null;
        [SerializeField] MeshRenderer meshRenderer = null;
        [SerializeField] AudioSource audioSource = null;

        private float currDistanceToHandRight;
        private float currDistanceToHandLeft;
        private float currTimeHeld;
        [SerializeField] bool isHandHovering = false;
        private void Update()
        {
            currDistanceToHandLeft = Vector3.Distance(transform.position, XRPlayer.LeftHand.Position);
            currDistanceToHandRight = Vector3.Distance(transform.position, XRPlayer.RightHand.Position);
            isHandHovering = currDistanceToHandRight < distanceToInteract || currDistanceToHandLeft < distanceToInteract;

            if (isHandHovering)
            {
                if (audioSource && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                currTimeHeld += Time.deltaTime;
            }
            else if (currTimeHeld > 0)
            {
                currTimeHeld -= Time.deltaTime;
            }
            else
            {
                if (audioSource)
                {
                    audioSource.Stop();
                }
                currTimeHeld = 0;
            }

            float ratio = currTimeHeld / timeToHold;

            //if source
            if (audioSource)
            {
                audioSource.volume = ratio;
            }

            ratio = Mathf.Clamp01(ratio);

            Color color = colorOverHoldTime.Evaluate(ratio);
            visualEffect.SetVector4("Color", color);

            //set the BaseColor of the meshRenderer to the color
            meshRenderer.material.SetColor("_BaseColor", color);

            if (ratio >= 1)
            {
                LoadSession();
            }
        }

        bool hasStartedLoading = false;
        public async void LoadSession()
        {
            if (hasStartedLoading)
                return;

            hasStartedLoading = true;

            await sessionLauncher.LoadSessionAsync();
            await UniTask.NextFrame();

            Destroy(transform.parent.gameObject);
        }


    }
}
