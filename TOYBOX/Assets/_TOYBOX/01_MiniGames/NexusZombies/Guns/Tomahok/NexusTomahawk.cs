using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine;
using Unity.VisualScripting;
using UnityEngine;
using Toolkit.Entity;

namespace Toolkit.NexusEngine
{
    public class NexusTomahawk : NexusWeapon
    {
        [SerializeField] private float returnTime = 3f;
        [SerializeField] private float throwForce = 10f;
        [SerializeField] private float throwTorque = 10f;
        [SerializeField] private bool isReturning = false;

        [SerializeField] private Rigidbody rb;
        Transform originalParent;
        protected override void OnAttack()
        {
            originalParent = transform.parent;
            _Throw();
        }

        private async void _Throw()
        {
            if (isReturning) return;
            rb.isKinematic = false;
            //throw the tomahawk forward with an impulse and torque and gradually slow down
            rb.AddForce(originalParent.forward * throwForce, ForceMode.Impulse);
            rb.AddTorque(originalParent.right * throwTorque, ForceMode.Impulse);
            isReturning = true;
            transform.parent = null;
            await UniTask.Delay((int)((returnTime * 1000) / 2));
            await ReturnTomahawk();
        }

        private async UniTask ReturnTomahawk()
        {
            Debug.Log("Returning Tomahawk");
            rb.velocity = Vector3.zero;
            rb.AddForce(-originalParent.forward * throwForce, ForceMode.Impulse);
            rb.AddTorque(-originalParent.right * throwTorque, ForceMode.Impulse);
            await UniTask.Delay((int)((returnTime * 1000) / 2));
            rb.velocity = Vector3.zero;
            isReturning = false;
            //reset to the original rotation and position
            rb.isKinematic = true;
            transform.parent = originalParent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isReturning && other.TryGetComponent(out IAttackable attackable) && other.gameObject != originalParent.gameObject)
            {
                Debug.Log("Attacking " + other.name);
                attackable.Attack(this);
            }
        }


    }
}
