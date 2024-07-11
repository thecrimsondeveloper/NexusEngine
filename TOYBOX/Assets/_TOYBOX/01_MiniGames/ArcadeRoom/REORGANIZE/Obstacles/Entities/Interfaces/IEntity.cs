using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.DependencyResolution;
using UnityEngine;


namespace ToyBox.Minigames.BeatEmUp
{
    public interface IEntity
    {
        UniTask Activate();
        UniTask Deactivate();


        public void ResolveDependencies(GameObject gameObject, EntityResolutionDefinition resolutionDefinition)
        {
            Debug.Log("Resolving dependencies on " + gameObject.name);
            // Resolve all dependencies on entity
            EntityResolver.ResolveDependencies(new ResolutionData
            {
                entity = this,
                gameObject = gameObject,
                resolutionDefinition = resolutionDefinition
            });
        }
    }
}
