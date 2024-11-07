using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class ChangeColorOfMeshRenderer : BaseSequence<ChangeColorOfMeshRendererData>
    {
        private MeshRenderer meshRenderer;
        private Color color;

        protected override UniTask Initialize(ChangeColorOfMeshRendererData currentData)
        {
            meshRenderer = currentData.meshRenderer;
            color = currentData.color;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            meshRenderer.material.color = color;
        }
    }

    public class ChangeColorOfMeshRendererData : BaseSequenceData
    {
        public MeshRenderer meshRenderer;
        public Color color;
    }
}
