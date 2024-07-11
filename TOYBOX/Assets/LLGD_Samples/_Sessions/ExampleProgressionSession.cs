using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.Sessions;

namespace Toolkit.Samples
{
    public class ExampleProgressionSession : ProgressionSession<Step>
    {
        ExampleProgressionSessionData exampleProgressionSessionData = null;
        protected override ProgressionSessionData ProgressionSessionData
        {
            get => exampleProgressionSessionData;
            set => exampleProgressionSessionData = value as ExampleProgressionSessionData;
        }


    }
}
