using UnityEngine;

public class SpectrumVisualizer : MonoBehaviour
{
    public AudioSpectrumHandler spectrumHandler;
    public Transform[] visualizers;

    [Header("Smoothing Settings")]
    public float smoothSpeed = 5f; // Controls how smooth the transitions are

    private float[] currentScales; // Store the current scale of each visualizer

    private void Start()
    {
        // Initialize the current scales array
        currentScales = new float[visualizers.Length];
    }

    private void Update()
    {
        if (spectrumHandler == null || visualizers.Length == 0) return;

        float[] bands = spectrumHandler.Bands;
        int bandCount = Mathf.Min(bands.Length, visualizers.Length);

        for (int i = 0; i < bandCount; i++)
        {
            float bandValue = bands[i];

            if (float.IsNaN(bandValue) || float.IsInfinity(bandValue) || bandValue < 0f)
                bandValue = 0f;

            // Adjust scale based on band index to increase effect for higher frequencies
            float targetScale = Mathf.Clamp(bandValue * (i + 1) * 10f, 0.2f, 15f);

            // Smoothly interpolate between the current scale and the target scale
            currentScales[i] = Mathf.Lerp(currentScales[i], targetScale, smoothSpeed * Time.deltaTime);

            // Apply the smoothed scale to the visualizer
            visualizers[i].localScale = new Vector3(1, currentScales[i], 1);
        }
    }
}
