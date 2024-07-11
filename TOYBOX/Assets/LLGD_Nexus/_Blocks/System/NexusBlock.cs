using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public abstract class NexusBlock : MonoBehaviour, INexusTrackable
    {
        protected NexusEntity entity;

        Guid INexusTrackable.guid { get; set; }

        private void OnValidate()
        {
            if (entity == null)
            {
                entity = GetComponent<NexusEntity>();
            }
            if (entity == null)
            {
                entity = GetComponentInParent<NexusEntity>();
            }
        }
        public void InitializeBlock(NexusEntity entity)
        {
            this.entity = entity;
            (this as INexusTrackable).InitializeTrackable();
            OnInitializeBlock(entity);
        }

        protected virtual void OnInitializeBlock(NexusEntity entity)
        {

        }

        public void ConnectBlock(NexusBlock block)
        {

        }

        public bool ResolveComponent<T>(ref T component) where T : Component
        {
            if (component != null)
            {
                return true;
            }


            if (TryGetComponent(out T comp))
            {
                component = comp;
            }
            else
            {
                component = gameObject.AddComponent<T>();
            }

            return component != null;
        }

    }
}
