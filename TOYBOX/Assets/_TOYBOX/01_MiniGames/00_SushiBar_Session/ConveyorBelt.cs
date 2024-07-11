using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private void OnCollisionStay(Collision other)
    {
        Debug.Log("Collision: " + other.gameObject.name);
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.velocity.magnitude < (speed * 0.8f))
                rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }
    }
}
