using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform teleportLocation;

    private void OnDrawGizmos()
    {
        if (teleportLocation == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(teleportLocation.position, 0.5f);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered the teleporter");
        other.transform.position = teleportLocation.position;
    }
}
