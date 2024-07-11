using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox
{
    [System.Serializable]
    public class PuzzleRoomSessionStep : Step
    {
        // public override void ActivateContainer(bool active)
        // {
        //     base.ActivateContainer(active);
        // }
        public List<IntercomData> introIntercomData = new List<IntercomData>();
    }
}
