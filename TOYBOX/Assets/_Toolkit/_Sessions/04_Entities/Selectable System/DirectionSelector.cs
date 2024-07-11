using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPR
{

    public class DirectionSelector : SelectorBase
    {
        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.forward * Range);
        }
    }
}
