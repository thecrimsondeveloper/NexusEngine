using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using UnityEngine;

namespace YourNamespace
{
    public class TemplateClass : TemplateBaseClass<TemplateClassData>
    {
        protected override UniTask Initialize(TemplateClassData currentData)
        {
            // Initialization logic here
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Logic to execute when the sequence begins
        }

        protected override UniTask Unload()
        {
            // Cleanup logic here
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class TemplateClassData
    {
        // Define data fields here
    }
}
