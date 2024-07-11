using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.DependencyResolution
{
    public class DependencyResolver : MonoBehaviour
    {
        enum ResolveMode
        {
            OnAwake,
            OnStart,
            OnEnable,
            Manual
        }

        [Title("Settings")]
        [SerializeField] ResolveMode resolveMode = ResolveMode.OnAwake;

        [Title("Dependencies")]
        [SerializeField, HideLabel, ListDrawerSettings(Expanded = true)]
        DependencyDefinition[] dependencies = null;

        protected virtual void Awake()
        {
            if (resolveMode == ResolveMode.OnAwake)
            {
                Resolve().Forget();
            }
        }

        protected virtual void Start()
        {
            if (resolveMode == ResolveMode.OnStart)
            {
                Resolve().Forget();
            }
        }

        protected virtual void OnEnable()
        {
            if (resolveMode == ResolveMode.OnEnable)
            {
                Resolve().Forget();
            }
        }

        [Title("Actions")]
        [Button, HideInEditorMode]
        public async UniTask Resolve()
        {
            await DependencyManager.ResolveDependencies(dependencies);
            await UniTask.NextFrame();
        }

        [Button, HideInEditorMode]
        public void ResetDependencies()
        {
            foreach (var dependency in dependencies)
            {
                dependency.Reset();
            }
        }


    }
}
