using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using ToyBox;
using UnityEngine;

namespace Toolkit.Sessions
{
    public class ProgressionSessionData : SessionData
    {
        [SerializeField] public bool playStepsOnStart = false;
        [SerializeField] public int startingStep = 0;
    }
}
