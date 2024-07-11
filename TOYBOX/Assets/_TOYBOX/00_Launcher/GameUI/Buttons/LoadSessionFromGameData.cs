using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Toolkit.Sessions;

namespace ToyBox
{
    public class LoadSessionFromGameData : TriggerButton
    {
        [SerializeField] GameData gameData;
        protected override void OnClickAction()
        {
            if (gameData == null)
                return;
            if (gameData.Sessions.Count > 0)
                Session.LoadFromData(gameData.Sessions[Random.Range(0, gameData.Sessions.Count)]);
        }

        protected override void OnResetButton()
        {

        }

        protected override void OnTriggerSqueeze(float squeezeValue)
        {

        }
    }
}
