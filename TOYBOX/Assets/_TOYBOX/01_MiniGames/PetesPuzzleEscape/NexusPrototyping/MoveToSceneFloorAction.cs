using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "MoveToSceneFloorAction", menuName = "Nexus/Actions/MoveToSceneFloorAction")]
    public class MoveToSceneFloorAction : NexusAction
    {
        public override void OnExecute()
        {
            mono.transform.position = XRPlayspace.Info.FloorPosition;
        }

        public override void SetupAction()
        {

        }

    }
}
