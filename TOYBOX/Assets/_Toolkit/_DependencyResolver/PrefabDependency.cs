using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;


namespace Toolkit.DependencyResolution
{
    [CreateAssetMenu(fileName = "PrefabDependency", menuName = "LLGD/Prefab Dependency")]
    public class PrefabDependency : DependencyDefinition<GameObject>
    {
        [Title("Settings")]
        [SerializeField, HideLabel] Pose pose = Pose.identity;
        [SerializeField] EntityResolutionDefinition entityResolutionDefinition = null;

        [Title("Debugging")]
        [ShowInInspector, ReadOnly] GameObject spawnedObject = null;

        protected override void Reset(GameObject resolver)
        {
            if (spawnedObject != null)
            {
                Destroy(spawnedObject);
            }
        }

        protected override async UniTask ResolveDependencies(GameObject resolver)
        {
            spawnedObject = Instantiate(resolver, pose.position, pose.rotation);
            await UniTask.NextFrame();
            await OnPrefabInstantiated(spawnedObject);

        }

        protected virtual async UniTask OnPrefabInstantiated(GameObject spawnedObject)
        {
            if (entityResolutionDefinition == null)
            {
                return;
            }
            Debug.Log("Resolving dependencies from  " + this.name + " to " + spawnedObject.name);
            if (spawnedObject.TryGetComponent(out IEntity entity))
            {
                Debug.Log("Resolving dependencies from " + this.name);
                entity.ResolveDependencies(spawnedObject, entityResolutionDefinition);
            }
        }




    }
}