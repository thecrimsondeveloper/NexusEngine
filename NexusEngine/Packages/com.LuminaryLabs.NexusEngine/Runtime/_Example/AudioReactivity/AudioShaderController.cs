using UnityEngine;
using System.Collections.Generic;

public class AudioShaderController : MonoBehaviour
{
    [SerializeField] private List<Renderer> targetRenderers;
    public float audioAmplitudeVal;
    public float glowIntensityVal;
    public Color glowColorVal;

    void Update()
    {
        foreach (var renderer in targetRenderers)
        {
            if (renderer != null)
            {
                renderer.material.SetFloat("_AudioAmplitude", audioAmplitudeVal);
                renderer.material.SetFloat("_GlowIntensity", glowIntensityVal);
                renderer.material.SetColor("_GlowColor", glowColorVal);
            }
        }
    }
}