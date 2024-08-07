using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace YourNamespace
{
    public class TemplateClassGeneric : TemplateBaseClass
    {
        protected override UniTask Initialize(object currentData = null)
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
    public class TemplateClassGenericData
    {
        // Define data fields here
    }
}
