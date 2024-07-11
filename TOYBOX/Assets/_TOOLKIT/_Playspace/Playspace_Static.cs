using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Style;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Toolkit.Playspace
{
    public static class Playspace
    {
        public static UnityEvent onPlayspaceChanged = new UnityEvent();

        private static IPlayspace playspace = null;



        public static Pose pose => playspace != null ? playspace.GetPlayspacePose() : Pose.identity;


        public static Component GetPlaySpace()
        {
            return playspace as Component;
        }

        public static void SetPlayspace(IPlayspace playspace)
        {
            if (Playspace.playspace == playspace)
                return;

            Playspace.playspace = playspace;
            onPlayspaceChanged?.Invoke();
        }
    }
}
