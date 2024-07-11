using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.DependencyResolution
{
    [System.Serializable]
    internal class DependencyCollection<T> where T : DependencyDefinition
    {
#if UNITY_EDITOR
        [ListDrawerSettings(Expanded = true, NumberOfItemsPerPage = 30)]
        [OnValueChanged(nameof(OnDependenciesChanged))]

#endif
        [SerializeField] List<T> dependencies = new List<T>();

#if UNITY_EDITOR


        void OnDependenciesChanged()
        {
            List<T> uniqueDependencies = new List<T>();
            for (int i = 0; i < dependencies.Count; i++)
            {
                if (!uniqueDependencies.Contains(dependencies[i]))
                {
                    uniqueDependencies.Add(dependencies[i]);
                }
            }
            dependencies = uniqueDependencies;
        }


#endif



        public void ResolveDependencies()
        {
            foreach (DependencyDefinition item in dependencies)
            {
                item.ResolveDependency();
            }
        }

        public void Reset()
        {
            foreach (var dependency in dependencies)
            {
                dependency.Reset();
            }
        }
    }
}
