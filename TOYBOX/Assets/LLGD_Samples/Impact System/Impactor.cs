using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit
{
    public abstract class Impactor : MonoBehaviour
    {
        void OnCollisionEnter(Collision other)
        {
            Debug.Log("Collision With " + other.gameObject.name, other.gameObject);
            IImpactable impactable = other.gameObject.GetComponent<IImpactable>();
            if (impactable != null)
            {
                Debug.Log("Found impactable on " + other.gameObject.name, other.gameObject);

                ImpactInfo impactInfo = new ImpactInfo();
                impactInfo.point = other.contacts[0].point;
                impactInfo.normal = other.contacts[0].normal;
                impactInfo.force = other.impulse.magnitude;
                impactInfo.impactor = this;
                impactInfo.direction = other.transform.position - transform.position;

                UpdateImpactInfo(impactInfo);

                impactable.OnImpact(impactInfo);
            }
            else
            {
                Debug.Log("No impactable found on " + other.gameObject.name, other.gameObject);
            }
        }

        protected abstract void UpdateImpactInfo(ImpactInfo impactInfo);
    }
}
