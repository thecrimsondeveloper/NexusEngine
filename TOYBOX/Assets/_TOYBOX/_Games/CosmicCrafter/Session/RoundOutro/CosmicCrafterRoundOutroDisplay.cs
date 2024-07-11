using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox
{
    public class CosmicCrafterRoundOutroDisplay : MonoBehaviour
    {

        [SerializeField] GameObject[] starRings;
        [SerializeField] GameObject[] dummyStars;

        // Start is called before the first frame update

        public async void ShowStars(int amount)
        {

            await UniTask.Delay(500);
            for (int i = 0; i < starRings.Length; i++)
            {
                starRings[i].transform.localScale = Vector3.one;
                starRings[i].SetActive(true);
                await UniTask.Delay(100);
            }
            await UniTask.Delay(250);



            for (int i = 0; i < amount; i++)
            {
                if (i >= dummyStars.Length)
                {
                    break;
                }

                await UniTask.Delay(100);
                dummyStars[i].SetActive(true);
            }

            await UniTask.Delay(3000);

            HideDisplay();
        }

        async void HideDisplay()
        {

            //shrink the stars
            for (int i = 0; i < dummyStars.Length; i++)
            {
                await UniTask.Delay(100);
                dummyStars[i].SetActive(false);
            }

            await UniTask.Delay(100);

            for (int i = 0; i < starRings.Length; i++)
            {
                Transform ring = starRings[i].transform;
                ring.DOScale(0, 0.25f);
                await UniTask.Delay(100);
            }
        }
    }
}
