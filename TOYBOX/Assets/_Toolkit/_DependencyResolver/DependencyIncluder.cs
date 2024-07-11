using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.DependencyResolution
{
    public class DependencyIncluder : MonoBehaviour
    {
        bool resolveOnStart = true;
        //create a label explaining what this script does
        [DisplayAsString(Overflow = false), HideLabel, BoxGroup("NOTE:")]
        public string Description = "This script allows you to mark dependencies as resolved." +
            " This is to be used when a dependency is already in" +
            " the scene, but you want to force the dependency to be resolved in the DependencyResolver.";

        //create a list of dependencies to be resolved
        public List<DependencyDefinition> Dependencies = new List<DependencyDefinition>();

        // Start is called before the first frame update
        void Start()
        {
            if (resolveOnStart)
            {
                foreach (var dependency in Dependencies)
                {
                    DependencyManager.IncludeDependency(dependency);
                }
            }
        }
    }
}
