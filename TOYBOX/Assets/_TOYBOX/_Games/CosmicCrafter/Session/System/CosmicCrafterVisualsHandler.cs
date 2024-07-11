using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicCrafterVisualsHandler : MonoBehaviour
    {
        [Title("Settings")]
        public Color successColor = Color.green;
        public Color failColor = Color.red;
        public int loops = 3;


        public List<MaterialVisuals> materialVisualReferences = new List<MaterialVisuals>();

        string backgroundColorName;

        private void Start()
        {
            for (int i = 0; i < materialVisualReferences.Count; i++)
            {
                MaterialVisuals visualReference = materialVisualReferences[i];
                visualReference.startColor = visualReference.sharedMaterial.GetColor(visualReference.colorName);
            }
        }

        [Button] void SendSuccess() => PulseColor(successColor);
        [Button] void SendFail() => PulseColor(failColor);

        Sequence GetMaterialPulseSequence(MaterialVisuals visualReference, Color color)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();

            Material material = visualReference.sharedMaterial;
            string colorName = visualReference.colorName;
            Color startColor = visualReference.startColor;

            float tweenToColorDuration = visualReference.tweenToColorDuration;
            Ease tweenToColorEase = visualReference.tweenToColorEase;

            float tweenBackToColorDuration = visualReference.tweenBackToColorDuration;
            Ease tweenBackToColorEase = visualReference.tweenBackToColorEase;

            if (material.HasColor(colorName) == false)
                Debug.LogError("Material " + material.name + " does not have a color property named " + colorName, material);
            else
                Debug.Log("Material " + material.name + " has a color property named " + colorName, material);

            sequence.Append(material.DOColor(color, colorName, tweenToColorDuration).SetEase(tweenToColorEase));
            sequence.Append(material.DOColor(startColor, colorName, tweenBackToColorDuration).SetEase(tweenBackToColorEase));

            sequence.SetLoops(loops);
            return sequence;
        }

        Sequence currentPulseSequence;
        public void PulseColor(Color color)
        {
            if (currentPulseSequence != null || currentPulseSequence.IsActive())
            {
                currentPulseSequence.Kill();
            }

            currentPulseSequence = DOTween.Sequence();
            foreach (MaterialVisuals visualReference in materialVisualReferences)
            {
                currentPulseSequence.Join(GetMaterialPulseSequence(visualReference, color));
            }


            // pulseSequence.Append(PulseMaterialColor(backgroundMaterial, backgroundColorName, color));

            currentPulseSequence.Play();
        }

        private void OnApplicationQuit()
        {
            Debug.Log("OnApplicationQuit");
            foreach (MaterialVisuals visualReference in materialVisualReferences)
            {
                visualReference.sharedMaterial.SetColor(visualReference.colorName, visualReference.startColor);
            }
        }


        [System.Serializable]
        public class MaterialVisuals
        {
            public Material sharedMaterial;
            public string colorName;

            [SerializeField] Color _startColor;
            public Color startColor
            {
                get => _startColor;
                set
                {
                    Debug.Log("Setting start color to " + value);
                    _startColor = value;
                }
            }


            [Space(10)]
            public float tweenToColorDuration;
            public Ease tweenToColorEase;

            [Space(10)]
            public float tweenBackToColorDuration;
            public Ease tweenBackToColorEase;
        }
    }
}
