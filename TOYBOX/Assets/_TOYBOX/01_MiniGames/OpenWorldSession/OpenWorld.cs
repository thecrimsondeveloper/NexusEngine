using System;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace ToyBox.Minigames.OpenWorld
{
    public class OpenWorld : MonoSequence
    {
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
            // Logic for after load
        }

        protected override void OnStart()
        {
            // Logic for start
        }

        protected override void OnFinished()
        {
            // Logic for when sequence is finished
        }

        protected override void OnUnload()
        {
            // Logic for unloading the sequence
        }
    }
}
