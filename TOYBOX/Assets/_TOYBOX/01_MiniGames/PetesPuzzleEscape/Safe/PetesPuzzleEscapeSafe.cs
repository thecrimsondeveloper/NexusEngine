using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Minigames.EscapeRoom
{
    public class PetesPuzzleEscapeSafe : MonoBehaviour
    {

        [Title("Component Dependencies")]
        [Required]
        [SerializeField] Extras.Keypad keypad;
        [SerializeField] private AudioSource audioSource;

        [Header("Asset Dependencies")]
        [Required]
        [SerializeField] private AudioClip accessDeniedSfx;
        [Required]
        [SerializeField] private AudioClip accessGrantedSfx;
        private bool accessWasGranted = false;


        private void Start()
        {
            keypad.WhenSubmit.AddListener(OnSubmit);
        }

        private async void OnSubmit(string value)
        {
            keypad.Clear();
        }
    }
}