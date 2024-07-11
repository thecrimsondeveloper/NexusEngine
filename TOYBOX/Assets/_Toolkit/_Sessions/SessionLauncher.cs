using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Sessions
{
    public class SessionLauncher : MonoBehaviour
    {

        [SerializeField] BaseSessionData sessionData = null;
        [SerializeField] bool loadOnStart = false;
        public UnityEvent onSessionLoaded;
        public UnityEvent onSessionUnloaded;


        private void Start()
        {
            if (loadOnStart)
                LoadSession();
        }
        [Button, HideInEditorMode]
        public void LoadSession()
        {
            LoadSessionAsync().Forget();
        }

        public async UniTask LoadSessionAsync()
        {
            await Load(sessionData);
            onSessionLoaded.Invoke();
        }

        [Button, HideInEditorMode]
        public async void UnloadSession()
        {
            if (Session.CurrentSession == sessionData)
                await Session.UnloadSession();
        }

        public static async UniTask Load(BaseSessionData sessionData)
        {
            if (sessionData is SessionData currSessionData)
            {
                await Session.LoadFromData(currSessionData);
            }
            else if (sessionData is ExternalSessionData currExternalSessionData)
            {
                await ExternalSession.LoadFromData(currExternalSessionData);
            }



        }

        public static async UniTask Unload(BaseSessionData sessionData)
        {
            if (sessionData is SessionData currSessionData)
            {
                await Session.UnloadSession();
            }
            else if (sessionData is ExternalSessionData currExternalSessionData)
            {
                await ExternalSession.UnloadAll();
            }
        }

    }
}
