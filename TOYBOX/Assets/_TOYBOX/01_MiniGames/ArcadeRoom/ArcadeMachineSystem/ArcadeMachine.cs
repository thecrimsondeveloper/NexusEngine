using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Minigames.BEUBArcadeRoom
{
    public class ArcadeMachine : MonoBehaviour
    {
        [SerializeField] Transform arcadeMachinePlayerPosition;
        [SerializeField] Transform boothTransform;
        [SerializeField] ExternalSession externalSession;
        [SerializeField] GameObject VREnvironmentContainer;
        [SerializeField] GameObject ArcadeVisualsContainer;
        [SerializeField] Animation vrAnimation;

        public UnityEvent<ArcadeMachine> OnVREnvironmentActivated = new UnityEvent<ArcadeMachine>();
        public UnityEvent<ArcadeMachine> OnVREnironmentDeactivated = new UnityEvent<ArcadeMachine>();


        bool isMachineHidden = false;
        bool isEnvironmentHidden = false;


        private void Start()
        {

            if (externalSession != null)
            {
                externalSession.OnSessionEndEvent.AddListener(TransitionToVR);
            }

            TransitionToArcade();
        }

        public void TransitionToArcade()
        {
            if (VREnvironmentContainer != null)
            {
                VREnvironmentContainer.SetActive(false);
                isEnvironmentHidden = true;

                OnVREnvironmentActivated.Invoke(this);

                XRPlayer.SetParent(null);
                XRPlayer.SetPosition(arcadeMachinePlayerPosition.position);
            }
        }

        public void TransitionToVR()
        {
            if (VREnvironmentContainer != null)
            {
                VREnvironmentContainer.SetActive(true);
                isEnvironmentHidden = false;
                OnVREnvironmentActivated.Invoke(this);

                XRPlayer.SetParent(boothTransform);
                XRPlayer.SetPosition(boothTransform.position + Vector3.up * 4);

                if (vrAnimation != null)
                {
                    vrAnimation.Play();
                }
            }
        }


        public void HideMachine()
        {
            //hide the arcade machine
            ArcadeVisualsContainer.SetActive(false);
            isMachineHidden = true;
        }

        public void ShowMachine()
        {
            ArcadeVisualsContainer.SetActive(true);
            isMachineHidden = false;
        }
    }
}
