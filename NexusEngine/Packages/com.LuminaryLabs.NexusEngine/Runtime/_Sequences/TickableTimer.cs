using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "TickableTimer", menuName = "Toolkit/TickableTimer")]
    public class TickableTimer : ScriptableSequence, ITickable
    {

        [SerializeField] int tickRate = 1;
        public int TickRate => tickRate;

        public void OnTick()
        {
            Debug.Log("Ticking");
        }

        protected override UniTask Finish()
        {
            throw new System.NotImplementedException();
        }

        protected override UniTask WhenLoad()
        {
            Tickables.Register(this);
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            Tickables.Unregister(this);
            return UniTask.CompletedTask;
        }
    }
}
