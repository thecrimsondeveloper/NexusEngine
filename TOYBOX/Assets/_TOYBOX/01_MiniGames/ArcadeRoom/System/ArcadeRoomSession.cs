using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.XR;
using Toolkit.Sessions;
using UnityEngine;
using ToyBox.Minigames.BEUBArcadeRoom;
using Sirenix.OdinInspector;

namespace ToyBox.Minigames.BeatEmUp
{

    public class ArcadeRoomSession : Session
    {
        ArcadeRoomSessionData sessionData = null;
        public override SessionData SessionData
        {
            get => sessionData;
            set => sessionData = value as ArcadeRoomSessionData;
        }

        [Title("Arcade Machines")]

        [SerializeField] List<ArcadeMachine> arcadeMachines = new List<ArcadeMachine>();

        [Title("Arcade Room Control")]
        [SerializeField] protected bool isArcadeRoomHidden = false;
        [SerializeField] List<GameObject> arcadeRoomObjects = new List<GameObject>();


        public override void OnSessionStart()
        {
            foreach (ArcadeMachine machine in arcadeMachines)
            {
                machine.OnVREnvironmentActivated.AddListener(OnVREnvironmentActivated);
                machine.OnVREnironmentDeactivated.AddListener(OnEnvironmentDeactivated);
            }
        }

        public void OnVREnvironmentActivated(ArcadeMachine sourceMachine)
        {
            HideArcadeRoom();
        }

        public void OnEnvironmentDeactivated(ArcadeMachine sourceMachine)
        {
            ShowArcadeRoom();
        }

        public override void OnSessionEnd()
        {

        }

        [Button("Hide Arcade Room")]
        public virtual void HideArcadeRoom()
        {

            //hide the arcade room
            foreach (GameObject obj in arcadeRoomObjects)
            {
                obj.SetActive(false);
            }

            //loop through all the arcade machines and hide them
            foreach (ArcadeMachine machine in arcadeMachines)
            {
                machine.HideMachine();
            }

            isArcadeRoomHidden = true;
        }

        [Button("Show Arcade Room")]
        public virtual void ShowArcadeRoom()
        {
            //show the arcade room
            foreach (GameObject obj in arcadeRoomObjects)
            {
                obj.SetActive(true);
            }

            //loop through all the arcade machines and show them
            foreach (ArcadeMachine machine in arcadeMachines)
            {
                machine.ShowMachine();
            }

            isArcadeRoomHidden = false;
        }

        [Button("Reset Arcade Room")]
        public virtual void ResetArcadeRoom()
        {
            //reset the arcade room
            ShowArcadeRoom();
        }
    }
}