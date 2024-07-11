using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox.Minigames.PetesPuzzleEscape
{
    public class TimerLightController : MonoBehaviour
    {

        [SerializeField] private List<VisualEffect> _visualEffects = new List<VisualEffect>();

        [Button("Set Color")]
        public void SetColor(Color col)
        {
            foreach (var vfx in _visualEffects)
            {
                vfx.SetVector4("Color", col);
            }

        }

        [Button("Set Intensity")]
        public void SetIntensity(float intensity)
        {
            foreach (var vfx in _visualEffects)
            {
                vfx.SetFloat("GlowIntensity", intensity);
            }
        }
#if UNITY_EDITOR

        [Button("Find All VisualEffects In Child")]
        void FindAllInChild()
        {
            _visualEffects.Clear();
            _visualEffects.AddRange(GetComponentsInChildren<VisualEffect>());
        }

#endif
    }
}
