using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.DependencyResolution;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Toolkit.Playspace
{

    [CreateAssetMenu(fileName = "PlayspaceDependency", menuName = "Toolkit/DependencyResolution/PlayspaceDependency")]
    public class PlayspaceDependency : PrefabDependency
    {
        [SerializeField] XRPlayspaceStyleProfile styleProfile;
        protected override async UniTask OnPrefabInstantiated(GameObject prefab)
        {
            IPlayspace playspace = prefab.GetComponent<IPlayspace>();
            if (playspace == null) playspace = prefab.GetComponentInChildren<IPlayspace>();


            if (playspace != null)
            {
                Playspace.SetPlayspace(playspace);

                await UniTask.WaitUntil(() => playspace.IsInitialized);
                await UniTask.DelayFrame(1);
                playspace.SetStyle(styleProfile);
            }
        }
    }
}
