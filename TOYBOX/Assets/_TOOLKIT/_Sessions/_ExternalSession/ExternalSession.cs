using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.DependencyResolution;
using UnityEngine;
using Toolkit.NexusEngine;
using UnityEngine.Events;

namespace Toolkit.Sessions
{
    public abstract class ExternalSession : MonoBehaviour
    {
        protected abstract ExternalSessionData ExternalSessionData { get; set; }

        // Add a bool value to determine if the session should start on load
        [SerializeField] bool startSessionOnLoad = false;
        float timeOnLoad = 0;
        float TotalSessionTime => Time.time - timeOnLoad;

        [HideInInspector] public UnityEvent OnSessionStartEvent = new UnityEvent();
        [HideInInspector] public UnityEvent OnSessionEndEvent = new UnityEvent();
        [HideInInspector] public UnityEvent OnSessionLoad = new UnityEvent();
        [HideInInspector] public UnityEvent OnSessionUnload = new UnityEvent();

        public virtual async UniTask Load(ExternalSessionData sessionData)
        {
            //if session data is null make one
            if (sessionData == null)
            {
                sessionData = new GenericExternalSessionData();
            }

            gameObject.SetActive(true);

            await UniTask.NextFrame();

            if (sessionData != null)
            {
                this.ExternalSessionData = sessionData;
            }

            float loadDelay = sessionData.loadDelay;
            await UniTask.Delay((int)(loadDelay * 1000));
            timeOnLoad = Time.time;

            await OnLoad();

            if (startSessionOnLoad)
            {
                SessionStart();
            }

            OnSessionStartEvent.Invoke();
        }

        public async virtual UniTask Unload()
        {
            SessionEnd();
            await OnUnload();

            if (ExternalSessionData != null)
                foreach (var dependency in ExternalSessionData.dependencies)
                {
                    await DependencyManager.ResetDependency(dependency);
                }



            gameObject.SetActive(false);

            OnSessionUnload.Invoke();
        }

        protected virtual UniTask OnLoad() { return UniTask.CompletedTask; }
        protected virtual UniTask PlayTutorial() { return UniTask.CompletedTask; }
        protected abstract void OnSessionStart();
        protected abstract void OnSessionEnd();
        protected virtual UniTask OnUnload() { return UniTask.CompletedTask; }

        public static List<ExternalSession> LoadedExternalSessions { get; private set; } = new List<ExternalSession>();
        public static List<ExternalSessionData> LoadedExternalSessionDatas { get; private set; } = new List<ExternalSessionData>();
        public static ExternalSessionData CurrentSessionData { get; private set; }
        public static ExternalSession CurrentSession { get; private set; }
        public async static UniTask LoadFromData(ExternalSessionData sessionData)
        {
            //loop through existing external sessions and unload them
            foreach (var externSession in LoadedExternalSessions)
            {
                await externSession.Unload();
            }

            if (sessionData.dependencies.Length > 0)
                await DependencyManager.ResolveDependencies(sessionData.dependencies);

            await UniTask.NextFrame();
            CurrentSession = Instantiate(sessionData.ExternalSessionPrefab);
            await UniTask.NextFrame();

            float xPos = 5000 * LoadedExternalSessions.Count;
            CurrentSession.transform.localPosition = new Vector3(xPos, 0, 0);

            LoadedExternalSessions.Add(CurrentSession);
            CurrentSessionData = sessionData;

            await CurrentSession.Load(sessionData);
        }

        public async static UniTask EnableSession(ExternalSession externalSession)
        {
            if (externalSession == null) return;

            //if not in the list, add it
            if (!LoadedExternalSessions.Contains(externalSession))
            {
                LoadedExternalSessions.Add(externalSession);
            }

            //loop through existing external sessions and unload them
            foreach (var externSession in LoadedExternalSessions)
            {
                if (externSession != externalSession)
                {
                    await externSession.Unload();
                }
            }

            //update the existing session variables
            CurrentSession = externalSession;
            CurrentSessionData = externalSession.ExternalSessionData;

            //load the session
            await externalSession.Load(externalSession.ExternalSessionData);
        }

        public async static UniTask UnloadAll()
        {
            foreach (var externSession in LoadedExternalSessions)
            {
                await externSession.Unload();
            }
        }

        // Session start function
        public void SessionStart()
        {
            OnSessionStart();
            OnSessionStartEvent.Invoke();
        }

        public void SessionEnd()
        {
            OnSessionEnd();
            OnSessionEndEvent.Invoke();
        }
    }
}
