using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox.Minigames.FeedingFrenzy
{
    public class Eated : NexusEntity
    {
        public override UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void Eat(Eater eater)
        {
            Debug.Log("Eated");
            Destroy(gameObject);
        }

        protected override void OnInitializeEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
