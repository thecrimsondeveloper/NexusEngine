using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Sequences;
using UnityEngine;

namespace ToyBox.Minigames.EscapeRoom
{
    public class PetesPuzzleEscapeSession : MonoSequence
    {
        [SerializeField] private Intercom intercom = null;
        [ShowInInspector] public ISequence[] subSequences { get; set; }

        protected override UniTask Finish()
        {
            // Logic for async finish
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            // Logic for async load
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            // Logic for async unload
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            // Logic to execute after load
        }

        protected override void OnStart()
        {
            // Logic to execute on start
        }

        protected override void OnFinished()
        {
            // Logic to execute when sequence is finished
        }

        protected override void OnUnload()
        {
            // Logic to execute on sequence unload
        }
    }
}
