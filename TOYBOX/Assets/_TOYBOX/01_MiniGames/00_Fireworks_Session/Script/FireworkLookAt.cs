using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkLookAt : MonoBehaviour
{
    public enum LookAtType
    {
        None,
        Target,
        Camera
    }

    [SerializeField] bool ignoreY = false;

    [SerializeField] LookAtType lookAtType = LookAtType.None;

    [SerializeField] Transform target = null;

    private void Update()
    {
        switch (lookAtType)
        {
            case LookAtType.None:
                break;
            case LookAtType.Target:
                transform.LookAt(target);
                if (ignoreY)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                break;
            case LookAtType.Camera:
                transform.LookAt(Camera.main.transform);
                if (ignoreY)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                break;
            default:
                break;
        }
    }
}
