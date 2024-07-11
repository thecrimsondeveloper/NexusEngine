using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class PipeConfiguration : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;

        private void Start()
        {
            Destroy(gameObject, 10f);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter");
            if (other.TryGetComponent(out BirdMovement bird))
            {
                Debug.Log("Bird scored");
                FloppyBird.ScoreUI.IncrementScore(1);
            }

        }

        private void FixedUpdate()
        {
            transform.localPosition += Vector3.forward * moveSpeed * Time.fixedDeltaTime;
        }
    }
}
