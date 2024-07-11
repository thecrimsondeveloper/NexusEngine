using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.DependencyResolution;
using UnityEngine;


namespace Toolkit.Sessions
{
    public interface ISession
    {
        SessionData SessionData { get; set; }
        float timeOnLoadStart { get; set; }
        float timeOnLoadEnd { get; set; }
        public float CurrentScore { get; set; }

        [ShowInInspector, FoldoutGroup("Session", 1000)]
        public float TotalSessionTime => timeOnLoadEnd - Time.time;

        public async UniTask Load()
        {
            await OnLoad();
        }
        public async UniTask Unload()
        {
            foreach (var dependency in SessionData.dependencies)
            {
                await DependencyManager.ResetDependency(dependency);
            }
            await OnUnload();
        }

        public virtual void StartSession()
        {
            OnSessionStart();
        }

        public UniTask OnLoad();

        public void OnSessionStart();
        public void OnSessionEnd();
        public UniTask OnUnload();
    }

}

