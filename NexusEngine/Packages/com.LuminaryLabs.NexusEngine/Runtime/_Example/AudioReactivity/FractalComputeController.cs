using UnityEngine;

public class FractalComputeController : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    public AudioSpectrumHandler audioHandler;

    private ComputeBuffer audioBandsBuffer;
    private int kernelIndex;

    [Header("Fractal Settings")]
    public float zoom = 1f;
    public Vector2 offset = Vector2.zero;
    public float colorIntensity = 1f;
    public Color baseColor = Color.blue;
    public Color accentColor = Color.red;

    [Header("Audio Settings")]
    public float audioAmplitude = 0f;
    public float amplitudeMultiplier = 1f;

    void Start()
    {
        InitializeRenderTexture();
        InitializeComputeShader();
    }

    void Update()
    {
        if (audioHandler == null || audioHandler.Bands == null) return;

        UpdateAudioData();
        SetShaderParameters();
        DispatchComputeShader();
    }

    private void InitializeRenderTexture()
    {
        renderTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32)
        {
            enableRandomWrite = true
        };
        renderTexture.Create();
    }

    private void InitializeComputeShader()
    {
        kernelIndex = computeShader.FindKernel("CSMain");
        audioBandsBuffer = new ComputeBuffer(audioHandler.numBands, sizeof(float));
        computeShader.SetTexture(kernelIndex, "Result", renderTexture);
    }

    private void UpdateAudioData()
    {
        audioBandsBuffer.SetData(audioHandler.Bands);
        audioAmplitude = CalculateAmplitude(audioHandler.Bands) * amplitudeMultiplier;
    }

    private float CalculateAmplitude(float[] bands)
    {
        float sum = 0f;
        foreach (float band in bands)
        {
            sum += band;
        }
        return sum / bands.Length;
    }

    private void SetShaderParameters()
    {
        computeShader.SetBuffer(kernelIndex, "audioBands", audioBandsBuffer);
        computeShader.SetFloat("audioAmplitude", audioAmplitude);
        computeShader.SetFloat("time", Time.time);
        computeShader.SetFloat("colorIntensity", colorIntensity);
        computeShader.SetVector("BaseColor", baseColor);
        computeShader.SetVector("AccentColor", accentColor);
    }

    private void DispatchComputeShader()
    {
        int threadGroupsX = renderTexture.width / 8;
        int threadGroupsY = renderTexture.height / 8;
        computeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
    }

    private void OnDestroy()
    {
        if (audioBandsBuffer != null) audioBandsBuffer.Release();
        if (renderTexture != null) renderTexture.Release();
    }
}