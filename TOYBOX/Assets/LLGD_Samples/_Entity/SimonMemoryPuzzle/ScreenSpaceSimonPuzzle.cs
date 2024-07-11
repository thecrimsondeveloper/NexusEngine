using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.UI;

namespace Toolkit.Samples
{
    public class ScreenSpaceSimonPuzzle : SimonMemoryPuzzle
    {


        [SerializeField] SimonButton[] simonButtons = new SimonButton[0];

        protected override void Start()
        {
            base.Start();
            foreach (var button in simonButtons)
            {
                button.AddClickListener(OnSimonButtonClicked);
            }
        }

        void OnSimonButtonClicked(SimonButton button)
        {
            //index of the button in the array
            int index = System.Array.IndexOf(simonButtons, button);
            //pass the index to the guess number method
            if (GuessNumber(index))
            {

            }
            else
            {
                //if the guess is incorrect, reset the sequence
                // ShowSimonSequence(sequenceCompletable.TargetSequence);
            }
        }



        protected override void OnGenerateSequence(int[] sequence)
        {
            // ShowSimonSequence(sequence);
        }

        async void ShowSimonSequence(NexusIntQueue sequence)
        {
            // for (int i = 0; i < sequenceCompletable.TargetSequence.Length; i++)
            // {
            //     await simonButtons[sequenceCompletable.TargetSequence[i]].Highlight();
            //     await UniTask.Delay(500);
            // }
        }


        protected override void OnSequenceComplete()
        {

        }
    }

    [System.Serializable]
    class SimonButton
    {
        public Button button;
        public Image renderer;
        public Color highlightColor = Color.gray;

        public async UniTask Highlight()
        {
            Color startCol = renderer.color;
            renderer.color = highlightColor;
            await UniTask.Delay(500);
            renderer.color = startCol;
        }

        public void AddClickListener(System.Action<SimonButton> action)
        {
            button.onClick.AddListener(() => action(this));
        }


    }
}
