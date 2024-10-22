using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{

    public class MaterialHandler : EntitySequence<MaterialHandlerData>
    {
        public enum UseCase
        {
            ReplaceMaterial,
            ModifyProperties,
            AddMaterial,
            RemoveMaterial
        }

        [SerializeField,] private UseCase changeMode;
        [SerializeField] private Material newMaterial;
        [SerializeField] private string propertyToChange;
        [SerializeField] private Color targetColor;
        [SerializeField] private float lerpDuration;

        [SerializeField] private List<Renderer> targetRenderers;

        protected override async UniTask Initialize(MaterialHandlerData currentData)
        {
            if(currentData.targetRenderers != null)
                targetRenderers = currentData.targetRenderers;
            changeMode = currentData.changeMode;
            if(currentData.newMaterial != null)
                newMaterial = currentData.newMaterial;
            if(currentData.propertyToChange != null)
                propertyToChange = currentData.propertyToChange;
            targetColor = currentData.targetColor;
            lerpDuration = currentData.lerpDuration;
            if(currentData.targetRenderers != null)
                targetRenderers = currentData.targetRenderers;

            await UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            ApplyMaterialChange();
        }

        private void ApplyMaterialChange()
        {
            foreach (Renderer target in targetRenderers)
            {
                if (target == null) continue;
                switch (changeMode)
                {
                    case UseCase.ReplaceMaterial:
                        ReplaceMaterial(target);
                        break;

                    case UseCase.ModifyProperties:
                        ChangeMaterialPropertyOverTime(target).Forget();
                        break;

                    case UseCase.AddMaterial:
                        AddMaterial(target);
                        break;

                    case UseCase.RemoveMaterial:
                        RemoveMaterial(target);
                        break;
                }
            }
        }

        private void ReplaceMaterial(Renderer renderer)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = newMaterial;
            }
            renderer.materials = materials;
        }

        private void AddMaterial(Renderer renderer)
        {
            List<Material> materials = new List<Material>(renderer.materials);
            materials.Add(newMaterial);
            renderer.materials = materials.ToArray();
        }

        private void RemoveMaterial(Renderer renderer)
        {
            List<Material> materials = new List<Material>(renderer.materials);
            if (materials.Contains(newMaterial))
            {
                materials.Remove(newMaterial);
                renderer.materials = materials.ToArray();
            }
        }

        private async UniTask ChangeMaterialPropertyOverTime(Renderer renderer)
        {
            float elapsedTime = 0f;
            Material materialInstance = renderer.material;  // Create a unique material instance for the target

            Color initialColor = materialInstance.GetColor(propertyToChange);
            while (elapsedTime < lerpDuration)
            {
                materialInstance.SetColor(propertyToChange, Color.Lerp(initialColor, targetColor, elapsedTime / lerpDuration));
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class MaterialHandlerData : SequenceData
    {
        public List<Renderer> targetRenderers;
        public MaterialHandler.UseCase changeMode;
        public Material newMaterial;
        public string propertyToChange = "_BaseColor";
        public Color targetColor = Color.white;
        public float lerpDuration = 1.0f;
    }
}

