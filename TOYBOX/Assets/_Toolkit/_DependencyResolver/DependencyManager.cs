using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toolkit.DependencyResolution
{


    [CreateAssetMenu(menuName = "LLGD/DependencyManager")]
    public static class DependencyManager
    {
        public static Dictionary<DependencyType, DependencyDefinition> singleDependencies = new Dictionary<DependencyType, DependencyDefinition>();
        public static List<DependencyDefinition> dynamicDependencies = new List<DependencyDefinition>();
        public static void IncludeDependency(DependencyDefinition dependency)
        {
            //if is a scalable dependency add it to the list
            if (dependency.type == DependencyType.SCALABLE)
            {
                if (dynamicDependencies.Contains(dependency) == false)
                {
                    dynamicDependencies.Add(dependency);
                }
            }
            else
            {
                //if is a single dependency add it to the dictionary and resolve it
                if (singleDependencies.ContainsKey(dependency.type) == false)
                {
                    singleDependencies.Add(dependency.type, dependency);
                }
            }
        }
        public static async UniTask ResolveDependency(DependencyDefinition dependency)
        {
            //if is a scalable dependency add it to the list
            if (dependency.type == DependencyType.SCALABLE)
            {
                if (dynamicDependencies.Contains(dependency) == false)
                {
                    dynamicDependencies.Add(dependency);
                    await dependency.ResolveDependency();
                }
            }
            else
            {
                //if is a single dependency add it to the dictionary and resolve it
                if (singleDependencies.ContainsKey(dependency.type) == false)
                {
                    singleDependencies.Add(dependency.type, dependency);
                    await dependency.ResolveDependency();
                }
            }
        }

        public static async UniTask ResolveDependencies(DependencyDefinition[] dependencies)
        {
            foreach (var dependency in dependencies)
            {
                await ResolveDependency(dependency);
            }
            await UniTask.NextFrame();
        }





        public static async UniTask Reset()
        {
            foreach (var dependency in singleDependencies.Values)
            {
                dependency.Reset();
            }

            await UniTask.NextFrame();
            singleDependencies.Clear();

            foreach (var dependency in dynamicDependencies)
            {
                dependency.Reset();
            }

            await UniTask.NextFrame();
            dynamicDependencies.Clear();
        }

        public static async UniTask ResetDependency(DependencyDefinition dependency)
        {
            if (dependency.type == DependencyType.SCALABLE)
            {
                if (dynamicDependencies.Contains(dependency))
                {
                    dynamicDependencies.Remove(dependency);
                    dependency.Reset();
                }
            }
            else
            {
                if (singleDependencies.ContainsKey(dependency.type))
                {
                    singleDependencies.Remove(dependency.type);
                    dependency.Reset();
                }
            }
        }



    }



    public enum DependencyType
    {
        PLAYER,
        PLAYSPACE,
        GAME_UI,
        External_Session,
        SCALABLE
    }
}