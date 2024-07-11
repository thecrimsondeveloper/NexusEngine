using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox.Games.PhantomCommand
{
    public class Scythe : MonoBehaviour
    {

        [Title("Dependencies")]

        [Required]
        [SerializeField] ScytheHead head;

        [Required]
        [SerializeField] GameObject hqPrefab;

        [Required]
        [SerializeField] PointableUnityEventWrapper events;

        [Title("Settings")]
        [SerializeField] bool shouldDeparent = true;

        [ShowIf(nameof(shouldDeparent))]



        private void OnValidate()
        {

            if (events == null)
            {
                events = GetComponent<PointableUnityEventWrapper>();
            }
            if (events == null)
            {
                events = gameObject.GetComponentInChildren<PointableUnityEventWrapper>();
            }


            if (events == null)
            {
                Debug.LogError("No PointableUnityEventWrapper found on Scythe", this);

                shouldDeparent = false;
            }

        }


        private void Start()
        {
            events.WhenSelect.AddListener(OnSelect);
            head.OnStab.AddListener(OnStab);
        }

        [Button]
        private void OnSelect(PointerEvent evt)
        {
            if (shouldDeparent)
            {
                bool hasParent = transform.parent == null;
                bool parentHasParent = transform.parent.parent == null;
                if (hasParent && parentHasParent)
                {
                    transform.SetParent(transform.parent.parent);
                }
                else
                {
                    transform.SetParent(null);
                }
            }
        }

        void OnStab()
        {
            SpawnHQ();
        }



        void SpawnHQ()
        {
            if (hqPrefab)
            {
                GameObject hq = Instantiate(hqPrefab, Vector3.zero, head.transform.rotation, transform.parent);
                hq.transform.localPosition = new Vector3(head.transform.position.x, 0, head.transform.position.z);
            }

            Destroy(gameObject);
        }





    }
}
