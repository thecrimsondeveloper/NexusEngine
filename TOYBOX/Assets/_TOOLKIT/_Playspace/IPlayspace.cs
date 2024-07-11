using System.Collections;
using System.Collections.Generic;
using Toolkit.Style;
using UnityEngine;

namespace Toolkit.Playspace
{
    public interface IPlayspace : IStyleable<XRPlayspaceStyleProfile>
    {
        public bool IsInitialized { get; }
        Pose GetPlayspacePose();


    }


}
