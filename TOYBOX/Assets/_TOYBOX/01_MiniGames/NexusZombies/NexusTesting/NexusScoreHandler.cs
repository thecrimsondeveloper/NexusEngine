using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Toolkit.Entity;
using Toolkit.Extras;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class NexusScoreHandler : MonoBehaviour
    {

        [SerializeField] float _score = 0;
        float score
        {
            get => _score;
            set
            {
                _score = value;
                if (scoreText) scoreText.text = "Score: " + _score.ToString("F0");
            }
        }
        [SerializeField] float _scoreMultiplier = 1;
        float scoreMultiplier
        {
            get => _scoreMultiplier;
            set
            {
                _scoreMultiplier = value;
                //round to 2 decimal place
                if (scoreMultiplierText) scoreMultiplierText.text = "x" + _scoreMultiplier.ToString("F2");
            }
        }
        [SerializeField] float multiplierDecayRate = 1f;
        [SerializeField] float multiplierAdditionAmount = 0.1f;
        [SerializeField] int scoreChain = 0;
        [SerializeField] float timeToBreakChain;
        [SerializeField] float timeAtLastScoreAdd = 0;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text scoreMultiplierText;

        public float Score => score;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AddScore(1);

            float timeSinceScoreAdd = Time.time - timeAtLastScoreAdd;

            if (timeSinceScoreAdd > 1)
            {
                scoreChain = 0;
            }


            //deplete the score multiplier 
            if (timeSinceScoreAdd > 2)
            {
                scoreMultiplier = Mathf.Lerp(scoreMultiplier, 1, 10 * Time.deltaTime * multiplierDecayRate);
            }
        }

        public void AddScore(float amount)
        {
            score += amount * scoreMultiplier;

            if (scoreChain > 5)
            {
                AddMultiplier(multiplierAdditionAmount);
            }

            scoreChain += 1;

            timeAtLastScoreAdd = Time.time;
        }

        public void RemoveScore(float amount)
        {
            score -= amount;
        }

        public void AddMultiplier(float amount)
        {
            //a 0 - 1 value that will be multiplied by the amount to add to the multiplier

            float addMultiplier = Mathf.Clamp01(MathF.Pow(scoreMultiplier, -1f) * 10);


            scoreMultiplier += (amount * addMultiplier);
        }

        public void RegisterWithScorable(IScoreable scoreable)
        {
            Debug.Log("Registering with scoreable");
            scoreable.OnScoreEvent.AddListener(AddScore);
        }

        public void UnregisterWithScorable(IScoreable scoreable)
        {
            scoreable.OnScoreEvent.RemoveListener(AddScore);
        }
    }
}
