using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Extras;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusVelocityReaction : NexusBlock
    {
        [SerializeField] Rigidbody rb;

        [BoxGroup("Angle Range")]
        [SerializeField, HideLabel, BoxGroup("Angle Range/X")] NumberRange xAngleRange = new NumberRange(-1, 1);
        [SerializeField, HideLabel, BoxGroup("Angle Range/Y")] NumberRange yAngleRange = new NumberRange(-1, 1);
        [SerializeField, HideLabel, BoxGroup("Angle Range/Z")] NumberRange zAngleRange = new NumberRange(-1, 1);


        private void Update()
        {

            //a -1 to 1 value
            float yVelocity = Mathf.Clamp(rb.velocity.y, -1, 1);

            float angle = yVelocity * 30;

            //clamp the rotation
            entity.transform.eulerAngles = new Vector3(0, 0, angle);




            // //clamp the rotation
            // Vector3 eulerAngles = entity.transform.eulerAngles;
            // eulerAngles.x = xAngleRange.Clamp(angle);
            // eulerAngles.y = yAngleRange.Clamp(angle);
            // eulerAngles.z = zAngleRange.Clamp(angle);



        }
    }
}
