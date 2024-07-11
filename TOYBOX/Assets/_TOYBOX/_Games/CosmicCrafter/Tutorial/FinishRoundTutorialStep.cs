using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace ToyBox
{
    public class FinishRoundTutorialStep : CosmicTutorialStep
    {
        [SerializeField] private TMP_Text tutorialText = null;
        [SerializeField] private int hideDelay = 30;


        private async void OnEnable()
        {
            await UniTask.Delay(hideDelay * 1000);
            if (IsTutorialActive)
            {
                tutorialText.gameObject.SetActive(false);
            }
        }
    }
}
