using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class AudioLooping : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource = null;
        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
