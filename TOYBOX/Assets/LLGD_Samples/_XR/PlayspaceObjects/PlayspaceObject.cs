using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Style;
using UnityEngine;

namespace Toolkit.Playspace
{
    public abstract class PlayspaceObject<T> : MonoBehaviour, IStyleable<T> where T : PlayspaceObjectStyleProfile
    {
        [SerializeField] protected T styleProfile;
        public T StyleProfile { get => styleProfile; set => styleProfile = value; }


        public void SetStyle(T styleProfile)
        {
            StyleProfile = styleProfile;
            (this as IStyleable<T>).ApplyStyle(styleProfile);
        }

        async UniTask IStyleable<T>.ApplyStyle(T styleProfile)
        {
            await UniTask.NextFrame();
            await ApplyObjectStyle(styleProfile);
        }


        protected abstract UniTask ApplyObjectStyle(T styleProfile);
    }







}
