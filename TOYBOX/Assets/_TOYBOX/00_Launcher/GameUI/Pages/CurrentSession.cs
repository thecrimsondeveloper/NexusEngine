using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox.Sessions
{
    public static class CurrentSession
    {
        public static string Title => Session.CurrentSessionData.sessionName;
        public static float Score => Session.CurrentSession.CurrentScore;
    }
}
