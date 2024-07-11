using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;




namespace CPR
{
    public class SelectableSphere : SelectableBase
    {
        [SerializeField, FoldoutGroup("Selectable Sphere")] TMP_Text text;
        [SerializeField, FoldoutGroup("Selectable Sphere")] float progress = 0;
        [SerializeField, FoldoutGroup("Selectable Sphere")]MeshRenderer meshRenderer;

        protected override void OnSelect(SelectorBase selector)
        {
            text.text = "SELECTED";
            meshRenderer.material.color = Color.green;
        }
        protected override void OnDeselect(SelectorBase selector)
        {
            text.text = "";
            meshRenderer.material.color = Color.white;
        }

        protected override void OnProgressChanged(float normalizedValue, SelectorBase selector)
        {
            progress = normalizedValue;
            text.text = "PROGRESS: " + progress.ToString();
            meshRenderer.material.color = Color.Lerp(Color.white,Color.cyan,normalizedValue);
        }
    }

}