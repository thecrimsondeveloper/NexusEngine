using System;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class BaseBeepHandler : BaseSequence<BaseBeepHandlerData>
{
    private AudioSource audioSource;

    // Private fields to store frequency and duration
    private float _frequency;
    private float _duration;

    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    protected override UniTask Initialize(BaseBeepHandlerData currentData)
    {
        // Assign the AudioSource to the private field
        audioSource = currentData.audioSource;
        // Store the data in private fields
        _frequency = currentData.frequency;
        _duration = currentData.duration;

        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
        // Use the private fields instead of directly accessing currentData
        PlayBeep(_frequency, _duration);
    }

    /// <summary>
    /// Plays a beep sound using an AudioSource.
    /// </summary>
    private async void PlayBeep(float frequency, float duration)
    {
        // Generate the beep tone using the stored frequency and duration
        AudioClip beepClip = CreateToneAudioClip(frequency, duration);
        audioSource.clip = beepClip;
        audioSource.Play();

        await UniTask.Delay((int)(duration * 1000));

        // Complete the sequence
        Complete();
    }

    /// <summary>
    /// Creates an AudioClip with a specific frequency and duration.
    /// </summary>
    private AudioClip CreateToneAudioClip(float frequency, float duration)
    {
        int sampleRate = 44100; // Standard audio sample rate
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        // Generate a sine wave tone
        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * t);
        }

        AudioClip clip = AudioClip.Create("Beep", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}

[Serializable]
public class BaseBeepHandlerData : BaseSequenceData
{
    public AudioSource audioSource; 
    public float frequency = 440f; // Frequency in Hz (A4 note)
    public float duration = 1f;    // Duration in seconds
}
