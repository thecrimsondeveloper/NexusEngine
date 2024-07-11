using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
#endif

namespace ToyBox.Minigames.Fireworks
{
    [System.Serializable]
    public class FireworkRequest
    {
        public enum RequestType
        {
            Red,
            Blue,
            Yellow,
        }

        [SerializeField] RequestType requestType = RequestType.Red;

        public RequestType Type
        {
            get => requestType;
            set
            {
                requestType = value;
                FireworksSession.Data.GetRequestPrefab(requestType);
            }
        }
        public FireworkRequest(RequestType requestType, bool complete)
        {
            this.requestType = requestType;
        }
    }
}

