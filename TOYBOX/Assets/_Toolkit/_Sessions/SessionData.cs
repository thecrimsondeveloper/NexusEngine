using System.Collections;
using System.Collections.Generic;
using Toolkit.DependencyResolution;
using Sirenix.OdinInspector;
using UnityEngine;
using Toolkit.Playspace;
using Toolkit.Style;
using static Toolkit.Sessions.Session;

namespace Toolkit.Sessions
{

    public abstract class BaseSessionData : ScriptableObject
    {
        public string sessionName = "New Session";
        public float loadDelay = 0.0f;
        public DependencyDefinition[] dependencies = null;
        public DependencyDefinition[] postLoadDependencies = null;
        //public StyleProfile playspaceStyleProfile;

#if UNITY_EDITOR

        [Button("LOAD"), HideInEditorMode] void Load() => SessionLauncher.Load(this);
        [Button("UNLOAD"), HideInEditorMode] void Unload() => SessionLauncher.Unload(this);

#endif
    }

    public abstract class SessionData : BaseSessionData
    {
        public MonoBehaviour SessionPrefab;

        private void OnValidate()
        {
            // if (SessionPrefab is ISession == false)
            // {
            //     SessionPrefab = null;
            // }
        }

#if UNITY_EDITOR
        [Button("DEBUG LOAD SESSION"), HideInEditorMode]
        void Load()
        {
            Session.LoadFromData(this);
        }

#endif
    }

    //create a menu item to create a new session data
    [CreateAssetMenu(menuName = "Toolkit/Session Data/External Session Data", fileName = "New External Session Data")]

    public abstract class ExternalSessionData : BaseSessionData
    {
        public ExternalSession ExternalSessionPrefab;
        public RenderTexture externalSessionRenderTexture;
    }

    public class GenericExternalSessionData : ExternalSessionData
    {

    }
}

