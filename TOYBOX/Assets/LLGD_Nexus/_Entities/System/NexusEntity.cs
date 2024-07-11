using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine.Flappy;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public abstract class NexusEntity : MonoBehaviour, IEntity, INexusTrackable
    {
        [SerializeField] Guid INexusTrackable.guid { get; set; }

        public struct InitializeData
        {
            public bool SetPosition { get; set; }
            public Transform parent { get; set; }
            public Space space { get; set; }
            public Vector3 position { get; set; }
        }


        public void InitializeEntity(InitializeData data = default)
        {
            (this as INexusTrackable).InitializeTrackable();

            if (data.parent != null)
                transform.SetParent(data.parent);


            if (data.SetPosition)
            {
                if (data.space == Space.Self)
                {
                    transform.localPosition = data.position;
                }
                else
                {
                    transform.position = data.position;
                }
            }



            OnInitializeEntity();
        }

        protected abstract void OnInitializeEntity();

        public virtual UniTask Activate()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
        public virtual UniTask Deactivate()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }


        public void ResolveComponent<T>(out T component) where T : Component
        {
            if (TryGetComponent(out T comp))
            {
                component = comp;
            }
            else
            {
                component = gameObject.AddComponent<T>();
            }
        }
    }


}

