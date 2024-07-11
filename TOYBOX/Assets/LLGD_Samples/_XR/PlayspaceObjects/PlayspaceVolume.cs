using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Playspace
{
    public class PlayspaceVolume : PlayspaceObject<PlayspaceVolumeProfile>
    {
        protected override UniTask ApplyObjectStyle(PlayspaceVolumeProfile styleProfile)
        {
            Debug.Log("Applying Playspace Volume Style");
            return UniTask.CompletedTask;
        }
    }
}
