using System.Collections;
using System.Collections.Generic;
using Toolkit.Style;
using UnityEngine;

namespace Toolkit.Playspace
{
    [CreateAssetMenu(fileName = "PlayspaceStyleProfile", menuName = "Toolkit/Playspace/PlayspaceStyleProfile")]
    public class XRPlayspaceStyleProfile : StyleProfile
    {
        public PlayspacePlaneProfile meshProfile;
        public PlayspaceVolumeProfile volumeProfile;
    }
}
