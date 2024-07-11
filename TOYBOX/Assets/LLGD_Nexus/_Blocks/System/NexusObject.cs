using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusObject : ScriptableObject, INexusTrackable
    {
        [ShowInInspector] Guid INexusTrackable.guid { get; set; }

        public string GetGuid()
        {
            return (this as INexusTrackable).guid.ToString();
        }

        [Button, HideInEditorMode]
        public void InitializeObject()
        {
            //get a new guid
            (this as INexusTrackable).InitializeTrackable();

            OnInitializeObject();
        }
        protected virtual void OnInitializeObject()
        {

        }

    }
}
