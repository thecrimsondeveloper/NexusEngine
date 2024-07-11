using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomGenerator : PhantomBuilding
    {

        [Space(10)]
        [Title("Generator Dependencies")]


        [Required]
        public AudioSource audioSource;
        [Required]
        public AudioClip onGenerateSound;

        [Title("Generator Settings")]
        public float generationAmount = 1.0f;
        [HideInPlayMode] public float generationRate = 1.0f;

        [Title("Generator Debug")]
        [ShowInInspector, HideInEditorMode, ReadOnly] float GenerationRate => generationRate;


        private void Awake()
        {
            if (audioSource == null)
            {
                Debug.LogError("PhantomGenerator: audioSource is not set!");
            }
        }

        private void Start()
        {
            InvokeRepeating(nameof(Generate), generationRate, generationRate);
        }

        void Generate()
        {
            owner.GivePower(generationAmount);
            if (onGenerateSound == null)
            {
                Debug.LogError("PhantomGenerator: onGenerateSound is not set!");
            }
            else
            {
                audioSource.PlayOneShot(onGenerateSound);
            }
        }


    }
}
