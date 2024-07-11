using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Toolkit.DependencyResolution;
using System.Collections.Generic;



namespace Toolkit.Sessions
{
    public abstract class Session : MonoBehaviour, ISession
    {


        [ShowInInspector, FoldoutGroup("Session", 1000), ShowIf(nameof(SessionData))]
        public virtual SessionData SessionData { get; set; } = null;

        [ShowInInspector, FoldoutGroup("Session", 1000)]
        public float timeOnLoadStart { get; set; } = 0;

        [ShowInInspector, FoldoutGroup("Session", 1000)]
        public float timeOnLoadEnd { get; set; } = 0;

        [ShowInInspector, FoldoutGroup("Session", 1000)]
        public float CurrentScore { get; set; } = 0;
        [ShowInInspector, FoldoutGroup("Session", 1000)]



        public virtual UniTask OnLoad() { return UniTask.CompletedTask; }
        public abstract void OnSessionStart();
        public abstract void OnSessionEnd();
        public virtual UniTask OnUnload() { return UniTask.CompletedTask; }









        #region Static Section
        [FoldoutGroup("Session", 1000), ShowInInspector, ShowIf(nameof(CurrentSessionData))]
        public static SessionData CurrentSessionData { get; private set; }
        [FoldoutGroup("Session", 1000), ShowInInspector, ShowIf(nameof(CurrentSessionData))]
        public static ISession CurrentSession { get; private set; }
        public static UnityEvent<SessionData> OnSessionLoad = new UnityEvent<SessionData>();
        public static UnityEvent<SessionData> OnSessionLoaded = new UnityEvent<SessionData>();
        public static UnityEvent OnSessionUnload = new UnityEvent();
        public static UnityEvent OnSessionUnloaded = new UnityEvent();

        public async static UniTask Load(Session session)
        {
            if (session.SessionData == null)
            {
                Debug.LogError("Session data is null.");
                return;
            }

            if (CurrentSession != null)
            {
                await UnloadSession();
            }

            if (session.SessionData.dependencies.Length > 0)
            {
                await DependencyManager.ResolveDependencies(session.SessionData.dependencies);
            }

            await UniTask.NextFrame();
            CurrentSession = session;
            await UniTask.NextFrame();

            CurrentSessionData = session.SessionData;
            OnSessionLoad.Invoke(CurrentSessionData);

            await LoadCurrentSession();

            if (session.SessionData.postLoadDependencies.Length > 0)
            {
                await DependencyManager.ResolveDependencies(session.SessionData.postLoadDependencies);
            }

            if (session.SessionData)
            {
                OnSessionLoaded.Invoke(session.SessionData);
            }

        }


        public async static UniTask LoadFromData(SessionData sessionData)
        {
            Debug.Log("Loading Session: " + sessionData);
            if (CurrentSession != null)
            {
                await UnloadSession();
            }

            if (sessionData.dependencies.Length > 0)
                await DependencyManager.ResolveDependencies(sessionData.dependencies);

            await UniTask.NextFrame();
            CurrentSession = Instantiate(sessionData.SessionPrefab) as ISession;
            CurrentSession.SessionData = sessionData;
            await UniTask.NextFrame();

            CurrentSessionData = sessionData;
            OnSessionLoad.Invoke(sessionData);

            Debug.Log("Session To Load:  " + sessionData, sessionData);

            await LoadCurrentSession();
            await UniTask.NextFrame();



            if (sessionData.postLoadDependencies.Length > 0)
                await DependencyManager.ResolveDependencies(sessionData.postLoadDependencies);

            OnSessionLoaded.Invoke(sessionData);
        }

        static async UniTask LoadCurrentSession()
        {
            if (CurrentSession == null)
            {
                Debug.LogError("No current session available.");
                return;
            }
            if (CurrentSession.SessionData == null)
            {
                Debug.LogError("Current session data is null.");
                return;
            }
            #region  load current session

            await UniTask.NextFrame();

            await UniTask.Delay((int)(CurrentSessionData.loadDelay * 1000));
            CurrentSession.timeOnLoadStart = Time.time;
            await CurrentSession.Load();
            CurrentSession.timeOnLoadEnd = Time.time;
            CurrentSession.StartSession();

            #endregion
        }



        public async static UniTask UnloadSession()
        {
            if (CurrentSession != null)
            {
                OnSessionUnload.Invoke();
                await CurrentSession.Unload();

                if (CurrentSession is MonoBehaviour mono)
                {
                    Destroy(mono.gameObject);

                }
                CurrentSession = null;
                CurrentSessionData = null;
                OnSessionUnloaded.Invoke();
            }
        }
        #endregion

    }
}
