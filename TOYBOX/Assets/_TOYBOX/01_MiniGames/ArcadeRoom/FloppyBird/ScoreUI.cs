using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI scoreText;
        int score = 0;



        public void IncrementScore(int amount)
        {
            score += amount;
            scoreText.text = score.ToString();
        }

        public void ResetScore()
        {
            score = 0;
            scoreText.text = score.ToString();
        }
    }
}
