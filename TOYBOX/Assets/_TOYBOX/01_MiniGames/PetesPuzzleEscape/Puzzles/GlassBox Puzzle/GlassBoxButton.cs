using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox
{
    public class GlassBoxButton : MonoBehaviour
    {
        [SerializeField] PointableUnityEventWrapper events;
        [SerializeField] GlassBoxFan fan;

        private bool isOn = false;

        private void Start()
        {
            events.WhenSelect.AddListener(OnSelect);
        }

        [Button("Select")]
        private void OnSelect(PointerEvent pointerEvent)
        {
            if (isOn)
            {
                fan.StopFan();
                isOn = false;
            }
            else
            {
                fan.RunFan();
                isOn = true;
            }
        }
    }
}
