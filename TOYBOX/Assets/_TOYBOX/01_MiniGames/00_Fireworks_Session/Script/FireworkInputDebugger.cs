using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ToyBox.Minigames.Fireworks
{
    public class FireworkInputDebugger : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                FireworkSpawner.SpawnFirework(FireworkRequest.RequestType.Red, transform.position, transform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                FireworkSpawner.SpawnFirework(FireworkRequest.RequestType.Blue, transform.position, transform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                FireworkSpawner.SpawnFirework(FireworkRequest.RequestType.Yellow, transform.position, transform);
            }
        }
    }
}
