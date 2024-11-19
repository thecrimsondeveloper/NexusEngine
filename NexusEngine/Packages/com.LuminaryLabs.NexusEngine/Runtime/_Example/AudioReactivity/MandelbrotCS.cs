using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MandelbrotCS : MonoBehaviour
{
     [Header("UI Reference")]
    public RawImage displayImage;  // Reference to the UI RawImage component
    
    [Header("Shader Settings")]
    public ComputeShader computeShader;
    
    [Header("Fractal Settings")]
    public int maxIterations = 100;
    public Vector2 position = new Vector2(-0.5f, 0f);
    public float zoom = 2.5f;
    
    [Header("Audio Reaction Settings")]
    [Range(0f, 2f)] public float audioAmplitudeInfluence = 1f;
    [Range(0f, 2f)] public float audioGlowInfluence = 0.5f;
    
    private AudioSpectrumHandler audioHandler;
    private ComputeBuffer dataBuffer;
    private ComputeBuffer audioBuffer;
    private RenderTexture renderTexture;
    
    private struct FractalData
    {
        public double w, h, r, i;
        public int screenWidth, screenHeight;
    }

    private void Start()
    {
        if (displayImage == null)
        {
            Debug.LogError("Please assign a RawImage component to the FractalUIController!");
            enabled = false;
            return;
        }

        audioHandler = GetComponent<AudioSpectrumHandler>();
        InitializeBuffers();
    }

    private void InitializeBuffers()
    {
        // Get the RawImage's rect dimensions
        RectTransform rectTransform = displayImage.rectTransform;
        Vector2 sizeDelta = rectTransform.rect.size;
        int width = Mathf.RoundToInt(sizeDelta.x);
        int height = Mathf.RoundToInt(sizeDelta.y);

        // Ensure minimum dimensions
        width = Mathf.Max(width, 1);
        height = Mathf.Max(height, 1);

        // Create render texture
        renderTexture = new RenderTexture(width, height, 0);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        // Assign render texture to RawImage
        displayImage.texture = renderTexture;

        // Initialize compute buffers
        dataBuffer = new ComputeBuffer(1, System.Runtime.InteropServices.Marshal.SizeOf<FractalData>());
        audioBuffer = new ComputeBuffer(audioHandler.numBands, sizeof(float));
    }

    private void Update()
    {
        if (computeShader == null || renderTexture == null) return;

        // Check if RawImage size has changed
        RectTransform rectTransform = displayImage.rectTransform;
        Vector2 currentSize = rectTransform.rect.size;
        if (currentSize.x != renderTexture.width || currentSize.y != renderTexture.height)
        {
            // Release old resources
            renderTexture.Release();
            
            // Reinitialize with new size
            InitializeBuffers();
        }

        // Update fractal data
        FractalData data = new FractalData
        {
            w = zoom,
            h = zoom * renderTexture.height / renderTexture.width,
            r = position.x,
            i = position.y,
            screenWidth = renderTexture.width,
            screenHeight = renderTexture.height
        };
        dataBuffer.SetData(new FractalData[] { data });

        // Calculate audio values
        float averageAmplitude = 0f;
        for (int i = 0; i < audioHandler.Bands.Length; i++)
        {
            averageAmplitude += audioHandler.Bands[i];
        }
        averageAmplitude /= audioHandler.Bands.Length;

        // Set compute shader parameters
        int kernelIndex = computeShader.FindKernel("CSMain");
        computeShader.SetBuffer(kernelIndex, "buffer", dataBuffer);
        computeShader.SetBuffer(kernelIndex, "audioSpectrum", audioBuffer);
        computeShader.SetTexture(kernelIndex, "Result", renderTexture);
        computeShader.SetInt("maxIterations", maxIterations);
        computeShader.SetFloat("audioAmplitude", averageAmplitude * audioAmplitudeInfluence);
        computeShader.SetFloat("audioGlowIntensity", averageAmplitude * audioGlowInfluence);

        // Update audio buffer
        audioBuffer.SetData(audioHandler.Bands);

        // Dispatch compute shader
        int threadGroupsX = Mathf.CeilToInt(renderTexture.width / 24.0f);
        int threadGroupsY = Mathf.CeilToInt(renderTexture.height / 24.0f);
        computeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
    }

    private void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            renderTexture = null;
        }
        
        if (dataBuffer != null)
        {
            dataBuffer.Release();
            dataBuffer = null;
        }
        
        if (audioBuffer != null)
        {
            audioBuffer.Release();
            audioBuffer = null;
        }
    }

    // Optional: Add methods for runtime control
    public void SetZoom(float newZoom)
    {
        zoom = newZoom;
    }

    public void SetPosition(Vector2 newPosition)
    {
        position = newPosition;
    }

    public void SetIterations(int iterations)
    {
        maxIterations = Mathf.Max(1, iterations);
    }
}
