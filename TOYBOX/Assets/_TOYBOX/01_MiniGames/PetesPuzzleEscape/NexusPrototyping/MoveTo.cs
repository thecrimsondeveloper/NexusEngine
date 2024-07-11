using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "MoveToSceneFloorAction", menuName = "Nexus/Actions/MoveTo")]
    public class MoveTo : NexusAction
    {
        [Title("Control")]
        [SerializeField] bool position = true;
        [SerializeField] bool rotate = false;
        [SerializeField] bool scale = false;

        [Title("Settings")]
        public Space space;

        [ShowIf(nameof(position)), BoxGroup("Position")] public Vector3 targetPosition;
        [ShowIf(nameof(position)), BoxGroup("Position")] public bool preservePositionX;
        [ShowIf(nameof(position)), BoxGroup("Position")] public bool preservePositionY;
        [ShowIf(nameof(position)), BoxGroup("Position")] public bool preservePositionZ;

        [ShowIf(nameof(rotate)), BoxGroup("Rotate")] public Vector3 targetRotation;
        [ShowIf(nameof(rotate)), BoxGroup("Rotate")] public bool preserveRotationX;
        [ShowIf(nameof(rotate)), BoxGroup("Rotate")] public bool preserveRotationY;
        [ShowIf(nameof(rotate)), BoxGroup("Rotate")] public bool preserveRotationZ;

        [ShowIf(nameof(scale)), BoxGroup("Scale")] public Vector3 targetScale;
        [ShowIf(nameof(scale)), BoxGroup("Scale")] public bool preserveScaleX;
        [ShowIf(nameof(scale)), BoxGroup("Scale")] public bool preserveScaleY;
        [ShowIf(nameof(scale)), BoxGroup("Scale")] public bool preserveScaleZ;



        public void MoveToPosition(MonoBehaviour mono)
        {
            Vector3 pos = space == Space.Self ? mono.transform.localPosition : mono.transform.position;
            if (preservePositionX == false) pos.x = targetPosition.x;
            if (preservePositionY == false) pos.y = targetPosition.y;
            if (preservePositionZ == false) pos.z = targetPosition.z;

            if (space == Space.Self)
            {
                mono.transform.localPosition = pos;
            }
            else
            {
                mono.transform.position = pos;
            }
        }

        public void MoveToRotation(MonoBehaviour mono)
        {
            Vector3 rot = space == Space.Self ? mono.transform.localEulerAngles : mono.transform.eulerAngles;
            if (preserveRotationX == false) rot.x = targetRotation.x;
            if (preserveRotationY == false) rot.y = targetRotation.y;
            if (preserveRotationZ == false) rot.z = targetRotation.z;

            if (space == Space.Self)
            {
                mono.transform.localEulerAngles = rot;
            }
            else
            {
                mono.transform.eulerAngles = rot;
            }
        }

        public void MoveToScale(MonoBehaviour mono)
        {
            Vector3 scale = mono.transform.localScale;
            if (preserveScaleX == false) scale.x = targetScale.x;
            if (preserveScaleY == false) scale.y = targetScale.y;
            if (preserveScaleZ == false) scale.z = targetScale.z;

            mono.transform.localScale = scale;
        }

        public override void SetupAction()
        {

        }

        public override void OnExecute()
        {
            if (position) MoveToPosition(mono);
            if (rotate) MoveToRotation(mono);
            if (scale) MoveToScale(mono);
        }
    }
}
