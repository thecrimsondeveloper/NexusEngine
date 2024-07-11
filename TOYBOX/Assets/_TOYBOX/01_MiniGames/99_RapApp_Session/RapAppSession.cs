using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Toolkit.Sessions;
using TMPro;

namespace ToyBox.Minigames.RapApp
{

    public class RapAppSession : RoundBasedSession
    {
        [SerializeField] Transform wordParent = null;
        [SerializeField] TMP_Text wordPrefab;
        RapAppSessionData sessionData = null;

        protected override RoundBasedSessionData RoundBasedSessionData
        {
            get => sessionData as RoundBasedSessionData;
            set => sessionData = value as RapAppSessionData;
        }

        public override UniTask OnLoad()
        {
            return UniTask.CompletedTask;
        }




        async UniTask SendWordGroup(WordGroup group)
        {
            foreach (string word in group.words)
            {
                TMP_Text spawnedWord = Instantiate(wordPrefab, wordParent);
                await UniTask.Delay(1000);
            }
        }



        public override void OnSessionStart()
        {

        }

        public override void OnSessionEnd()
        {

        }

        public override UniTask OnUnload()
        {
            return UniTask.CompletedTask;
        }

        protected override RoundStats SetupStats(RoundStats currentRoundStats)
        {
            throw new System.NotImplementedException();
        }

        protected override UniTask OnRoundSetup(RoundDefinition round)
        {
            throw new System.NotImplementedException();
        }

        protected override UniTask OnRoundStart(Round round)
        {
            throw new System.NotImplementedException();
        }

        protected override UniTask OnRoundEnd(RoundStats round)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnRoundBasedSessionStart()
        {
            throw new System.NotImplementedException();
        }
    }

}