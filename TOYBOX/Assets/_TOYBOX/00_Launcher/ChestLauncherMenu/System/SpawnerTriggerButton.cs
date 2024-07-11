using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace ToyBox.Launcher
{

    public class SpawnerTriggerButton : TriggerButton
    {
        [SerializeField] ToySpawner spawner;
        private void Start()
        {

        }
        protected async override void OnClickAction()
        {
            spawner.SpawnToy();
        }
        protected override void OnTriggerSqueeze(float squeezeValue)
        {

        }
        protected override void OnResetButton()
        {

        }
    }
}