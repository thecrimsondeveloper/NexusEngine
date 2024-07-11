using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.DependencyResolution
{

    [CreateAssetMenu(menuName = "LLGD/Scene Depenceny")]
    class SceneDependency : DependencyDefinition<GameScene>
    {
        protected override void Reset(GameScene resolver)
        {
            resolver.UnloadScene();
        }

        protected override async UniTask ResolveDependencies(GameScene resolver)
        {
            resolver.LoadScene();
            await UniTask.WaitUntil(() => resolver.IsSceneLoaded);
        }
    }
}