using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.NexusEngine;
using Cysharp.Threading.Tasks;
using Toolkit.XR;
using Sirenix.OdinInspector;

namespace ToyBox
{
    public class XRTeleporter : NexusEntity
    {

        public NexusEventBlock OnLookAt = new NexusEventBlock();
        public NexusEventBlock OnLookAway = new NexusEventBlock();
        public NexusXRPlayer nexusXRPlayer;
        [SerializeField] bool isLookingAt = false;

        protected override void OnInitializeEntity()
        {
            OnLookAt.AddListener(LookAt);
            OnLookAway.AddListener(LookAway);
            nexusXRPlayer.OnPinch.AddListener(TryTeleport);
        }

        void LookAt()
        {
            isLookingAt = true;
        }

        void LookAway()
        {
            isLookingAt = false;
        }

        [Button("Teleport")]
        public void TryTeleport()
        {
            if (isLookingAt)
            {
                TeleportPlayer();
            }
        }



        public override UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void TeleportPlayer()
        {
            XRPlayer.MovePlayer(transform.position);
        }
    }
}
