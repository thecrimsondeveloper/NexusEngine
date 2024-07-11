using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.DependencyResolution
{
    public abstract class DependencyDefinition<T> : DependencyDefinition
    {
        [Title("Resolver")]
        [SerializeField, HideLabel] T resolver;

        public T Resolver => resolver;

        protected override async UniTask OnResolve()
        {
            if (resolver == null)
            {
                Debug.LogError($"Resolver is null on {name}", this);
                return;
            }
            await ResolveDependencies(resolver);
        }

        protected override void OnReset()
        {
            if (resolver == null)
            {
                Debug.LogError($"Resolver is null on {name}", this);
                return;
            }
            Reset(resolver);
        }

        protected abstract void Reset(T resolver);
        protected abstract UniTask ResolveDependencies(T resolver);


        internal void SetResolver(T resolver)
        {
            this.resolver = resolver;
        }
    }

    public abstract class DependencyDefinition : ScriptableObject
    {
        [Title("Dependency Ttpe"), HideLabel]
        public DependencyType type = DependencyType.SCALABLE;

        bool isResolved = false;

        private void OnValidate()
        {
            isResolved = false;
        }

        internal async UniTask ResolveDependency(bool force = false)
        {
            if (isResolved && force == false)
                return;
            isResolved = true;
            await OnResolve();
        }
        internal async void Reset()
        {
            isResolved = false;
            OnReset();
        }
        protected abstract UniTask OnResolve();
        protected abstract void OnReset();
    }








}

