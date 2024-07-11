using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace ToyBox
{
    public class SongPlayer : MonoBehaviour
    {

        [SerializeField] List<AudioClip> songs = new List<AudioClip>();
        [SerializeField] AudioSource audioSource = null;
        [SerializeField] AudioMixer audioMixerGroup = null;
        [SerializeField] string mixerParameterName = "FrequencyCutOff";
        Queue<AudioClip> songQueue = new Queue<AudioClip>();

        private void Awake()
        {
            foreach (var song in songs)
            {
                songQueue.Enqueue(song);
            }
        }

        private void Update()
        {
            if (!audioSource.isPlaying)
            {
                PlayNextSong();
            }

        }

        [Button]
        void PlayNextSong()
        {
            //get the first song in the queue
            AudioClip nextSong = songQueue.Dequeue();
            //add it to the end of the queue
            songQueue.Enqueue(nextSong);
            //set the audio source to play the next song
            audioSource.clip = nextSong;
            audioSource.Play();
        }

        [Button]
        void ToggleMixerLowPassEffect(bool toggle)
        {
            Debug.Log("Toggling Low Pass Effect");
            if (toggle)
            {
                audioMixerGroup.SetFloat(mixerParameterName, 200);
            }
            else
            {
                audioMixerGroup.SetFloat(mixerParameterName, 22000);
            }
        }
    }
}
