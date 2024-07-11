using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox
{
    public class FloppyBird : ExternalSession
    {
        [SerializeField] PipeSpawner pipeSpawner;
        [SerializeField] BirdMovement player;
        [SerializeField] ScoreUI scoreUI;

        public static BirdMovement Player { get; set; }
        public static ScoreUI ScoreUI { get; set; }
        protected override ExternalSessionData ExternalSessionData { get; set; }

        private void Start()
        {
            player.FloppyBird = this;
            Player = player;
            ScoreUI = scoreUI;
        }

        private void Update()
        {

        }


        public void Reset()
        {
            scoreUI.ResetScore();
            pipeSpawner.spawnedPipeConfigurations.Clear();
            StartCoroutine(pipeSpawner.ResetPipes());
        }

        protected override void OnSessionStart()
        {
        }

        protected override void OnSessionEnd()
        {
        }
    }
}
