using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sessions;
using UnityEngine;

namespace Toolkit.Sessions
{

    public class ExternalSessionDemo : ExternalSession//, IExternalSession
    {

        protected override ExternalSessionData ExternalSessionData { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        protected override void OnSessionEnd()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSessionStart()
        {
            throw new System.NotImplementedException();
        }
    }
}


