using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Playspace
{
    public class PlayspacePlane : PlayspaceObject<PlayspacePlaneProfile>
    {
        [SerializeField] LineRenderer edgeRenderer;
        [SerializeField] OVRScenePlane scenePlane;
        [SerializeField] GameObject edgeObject;
        protected override async UniTask ApplyObjectStyle(PlayspacePlaneProfile styleProfile)
        {
            Debug.Log("Applying Playspace Plane Style");
            await UniTask.DelayFrame(1);

            if (scenePlane == null)
            {
                GetComponent<OVRScenePlane>();
            }
            await UniTask.DelayFrame(1);

            if (scenePlane != null)
            {
                List<Vector3> edgePoints = new List<Vector3>();

                foreach (var point in scenePlane.Boundary)
                {
                    edgePoints.Add(point);
                }

                edgeRenderer.positionCount = edgePoints.Count;
                edgeRenderer.SetPositions(edgePoints.ToArray());
                edgeRenderer.loop = true;
                edgeRenderer.colorGradient = styleProfile.edgeColorGradient;
            }
        }
    }
}
