using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Toolkit.Sequences;
using UnityEngine;
using UnityEngine.VFX;
using Sequence = Toolkit.Sequences.Sequence;

namespace ToyBox.Minigames.EscapeRoom
{
    public class TowersOfHanoi : MonoSequence
    {
        [SerializeField] List<RingBase> towers = new List<RingBase>();
        [SerializeField] List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        public VisualEffect solveVFX;

        private void Start()
        {
            // Optional start logic if needed
        }

        protected override UniTask Finish()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material.SetFloat("_WipeAmount", 0);
                meshRenderer.material.SetColor("_Tint", new Color(0, 0, 0, 1));
            }

            foreach (RingBase tower in towers)
            {
                tower.OnComplete.AddListener(() => Sequence.Finish(this));
                tower.enabled = true;
            }

            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            foreach (RingBase tower in towers)
            {
                tower.OnComplete.RemoveListener(() => Sequence.Finish(this));
            }

            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            // Optional logic to execute after load if needed
        }

        protected override void OnStart()
        {
            // Optional logic to execute on start if needed
        }

        protected override void OnFinished()
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material.DOFloat(1, "_WipeAmount", 1f);
                meshRenderer.material.DOColor(new Color(0, 0, 0, 0), "_Tint", 1f);
            }
            solveVFX.Play();

            foreach (RingBase tower in towers)
            {
                tower.enabled = false;
            }
        }

        protected override void OnUnload()
        {
            // Optional logic to execute on unload if needed
        }
    }
}
