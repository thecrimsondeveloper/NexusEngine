using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using ToyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.DependencyResolution
{
    public abstract class PrerequisiteDependency : DependencyDefinition<RuntimePrerequisiteHandler>
    {
        bool suspendGameDuringPrerequisite = true;

        [Title("Debugging")]
        [ShowInInspector, ReadOnly] RuntimePrerequisiteHandler prereqResolver = null;


        protected override async UniTask ResolveDependencies(RuntimePrerequisiteHandler resolver)
        {
            prereqResolver = Instantiate(resolver);
            prereqResolver.OnPrerequisiteCompleted.AddListener(PrerequisiteComplete);
        }

        void PrerequisiteComplete()
        {
            OnPrerequisiteComplete();
            Reset();
        }

        protected override void Reset(RuntimePrerequisiteHandler resolver)
        {
            Destroy(prereqResolver.gameObject);
        }

        [Title("Prerequisite Dependency")]
        protected abstract void OnPrerequisiteComplete();
    }


}
