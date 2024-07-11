using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.XR
{
    public class XRPlayerVisuals : MonoBehaviour
    {
        [SerializeField] Camera camera = null;
        OVRManager ovrManager;
        private void Start()
        {
            if (ovrManager == null)
            {
                ovrManager = GetComponent<OVRManager>();
            }
        }
        public void EnablePassthrough()
        {
            if (ovrManager != null)
            {
                ovrManager.isInsightPassthroughEnabled = true;
            }

            if (camera != null)
            {
                //set it so the camera renders color
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0, 0, 0, 0);
            }
        }
        public void DisablePassthrough(Material skybox = null)
        {
            if (ovrManager != null)
            {
                ovrManager.isInsightPassthroughEnabled = false;
            }

            if (camera != null && skybox != null)
            {
                //set it so the camera renders color
                camera.clearFlags = CameraClearFlags.Skybox;
                RenderSettings.skybox = skybox;
            }
        }



    }
}
