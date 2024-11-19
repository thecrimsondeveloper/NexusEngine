using UnityEngine;
using UnityEngine.UI;

public class SimpleAudioFractalController : MonoBehaviour
{
    public ComputeShader computeShader;
    public RawImage displayImage;

    [Header("Fractal Settings")]
    public float zoom = 1.0f;
    public Vector2 offset = Vector2.zero;
    [Range(0.1f, 5.0f)]
    public float shapeReactivity = 1.0f;

    [Header("Smoothing Settings")]
    [Range(0.0f, 1.0f)]
    public float smoothingFactor = 0.9f; // Higher = smoother transitions

    [Header("Color Settings")]
    [Range(0.1f, 5.0f)]
    public float colorReactivity = 1.0f;

    private AudioSpectrumHandler audioHandler;
    private RenderTexture renderTexture;
    private ComputeBuffer dataBuffer;

    private float[] smoothedAudioBands;
    private float[] previousAudioBands;

    private struct ShaderData
    {
        public float zoom;
        public float offsetX;
        public float offsetY;
        public float audioIntensity;

        public float band0, band1, band2, band3, band4, band5, band6, band7;

        public float smoothingFactor;
        public float colorReactivity;
        public float padding1, padding2; // Ensure 16-byte alignment
    }

    void Start()
    {
        audioHandler = GetComponent<AudioSpectrumHandler>();
        InitializeRenderTexture();
        InitializeBuffers();
    }

    void InitializeRenderTexture()
    {
        if (renderTexture != null) renderTexture.Release();

        renderTexture = new RenderTexture(512, 512, 0);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        displayImage.texture = renderTexture;
    }

    void InitializeBuffers()
    {
        int bandCount = 8; // Assuming 8 audio bands
        smoothedAudioBands = new float[bandCount];
        previousAudioBands = new float[bandCount];

        dataBuffer = new ComputeBuffer(1, sizeof(float) * 16); // Adjusted for the ShaderData struct
    }

    void Update()
    {
        if (computeShader == null || renderTexture == null) return;

        // Smooth the audio data
        UpdateSmoothing(audioHandler.Bands);

        // Calculate average audio intensity
        float audioIntensity = 0f;
        for (int i = 0; i < smoothedAudioBands.Length; i++)
        {
            audioIntensity += smoothedAudioBands[i];
        }
        audioIntensity /= smoothedAudioBands.Length;

        // Update shader data
        ShaderData data = new ShaderData
        {
            zoom = zoom,
            offsetX = offset.x,
            offsetY = offset.y,
            audioIntensity = audioIntensity * shapeReactivity,
            band0 = smoothedAudioBands[0],
            band1 = smoothedAudioBands[1],
            band2 = smoothedAudioBands[2],
            band3 = smoothedAudioBands[3],
            band4 = smoothedAudioBands[4],
            band5 = smoothedAudioBands[5],
            band6 = smoothedAudioBands[6],
            band7 = smoothedAudioBands[7],
            smoothingFactor = smoothingFactor,
            colorReactivity = colorReactivity
        };

        dataBuffer.SetData(new ShaderData[] { data });

        // Set shader parameters
        int kernel = computeShader.FindKernel("CSMain");
        computeShader.SetBuffer(kernel, "dataBuffer", dataBuffer);
        computeShader.SetTexture(kernel, "Result", renderTexture);

        // Dispatch shader
        int threadGroupsX = Mathf.CeilToInt(renderTexture.width / 24.0f);
        int threadGroupsY = Mathf.CeilToInt(renderTexture.height / 24.0f);
        computeShader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
    }

    void UpdateSmoothing(float[] rawBands)
    {
        for (int i = 0; i < rawBands.Length; i++)
        {
            smoothedAudioBands[i] = Mathf.Lerp(previousAudioBands[i], rawBands[i], 1.0f - smoothingFactor);
            previousAudioBands[i] = smoothedAudioBands[i];
        }
    }

    void OnDestroy()
    {
        if (renderTexture != null) renderTexture.Release();
        if (dataBuffer != null) dataBuffer.Release();
    }
}
