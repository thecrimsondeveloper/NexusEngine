using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpectrumHandler : MonoBehaviour
{
    [Header("Audio Spectrum Settings")]
    [Range(64, 8192)] public int spectrumSize = 512; // Number of samples
    [Range(0f, 100f)] public float intensityMultiplier = 2.0f; // Scale the output for visual effects

    [Header ("Audio Shader Settings")]
    public float amplitudeMultiplier = 1.0f; // Scale the amplitude for shader effects
    public float glowMultiplier = 1.0f; // Scale the glow intensity for shader effects
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris; // Type of FFT window to use

    [Header("Refined Spectrum Settings")]
    public int numBands = 8; // Number of refined bands for visual effects
    public float smoothSpeed = 0.5f; // Smoothing speed for band values


    private AudioSource audioSource;
    private float[] rawSpectrumData;
    private float[] refinedBands;
    private float[] smoothedBands;

    // Refined band values for external use
    public float[] Bands => smoothedBands;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        numBands = Mathf.Min(numBands, spectrumSize);
        rawSpectrumData = new float[spectrumSize];
        refinedBands = new float[numBands];
        smoothedBands = new float[numBands];
    }

    private void Update()
    {
        audioSource.GetSpectrumData(rawSpectrumData, 0, fftWindow);
        RefineSpectrumData();

            // Calculate average amplitude from all bands
        float totalAmplitude = 0f;
        for (int i = 0; i < smoothedBands.Length; i++)
        {
            totalAmplitude += smoothedBands[i];
        }
        totalAmplitude /= smoothedBands.Length;

        // Update shader controller values
        
        // Option 1: Simple color based on amplitude
        
        // Option 2: If using gradient
        // shaderController.glowColorVal = audioColorGradient.Evaluate(totalAmplitude);
    }

    private void RefineSpectrumData()
    {
        float noiseThreshold = 0.001f; // Adjusted noise threshold for better filtering
        float minValidValue = 1e-6f;   // Smallest value considered valid to prevent division by near-zero
        float amplificationFactor = 10000f; // Increase to amplify low values

        // Reset bands
        for (int i = 0; i < numBands; i++)
        {
            refinedBands[i] = 0f;
            smoothedBands[i] = 0f;
        }

        // Logarithmic frequency bands calculation
        for (int i = 0; i < numBands; i++)
        {
            float sum = 0f;
            int startIdx = Mathf.FloorToInt(Mathf.Pow(i / (float)numBands, 2) * spectrumSize);
            int endIdx = Mathf.FloorToInt(Mathf.Pow((i + 1) / (float)numBands, 2) * spectrumSize);

            // Ensure indices are within bounds
            startIdx = Mathf.Clamp(startIdx, 0, spectrumSize - 1);
            endIdx = Mathf.Clamp(endIdx, startIdx + 1, spectrumSize);

            // Sum up the spectrum data for this band
            for (int j = startIdx; j < endIdx; j++)
            {
                sum += rawSpectrumData[j];
            }

            // Calculate the average, handling edge cases
            int sampleCount = endIdx - startIdx;
            float average = (sampleCount > 0) ? sum / sampleCount : 0f;

            // Apply amplification and noise threshold
            average *= amplificationFactor;
            if (average < noiseThreshold || float.IsNaN(average) || float.IsInfinity(average))
            {
                average = 0f;
            }

            // Store in the bands array
            refinedBands[i] = average;

            // Smooth the bands for visual effects
            smoothedBands[i] = Mathf.Lerp(smoothedBands[i], refinedBands[i], intensityMultiplier * smoothSpeed * Time.deltaTime);
        }
    }




    

}
