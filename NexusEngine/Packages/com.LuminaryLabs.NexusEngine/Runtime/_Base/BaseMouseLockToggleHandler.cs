using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseMouseLockToggleHandler : BaseSequence<BaseMouseLockToggleHandlerData>
    {
        protected override UniTask Initialize(BaseMouseLockToggleHandlerData currentData)
        {
            
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }

    [System.Serializable]
    public class BaseMouseLockToggleHandlerData : BaseSequenceData
    {

    }
}
