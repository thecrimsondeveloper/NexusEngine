using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Sessions
{
    public class LoadSessionGrabbable : MonoBehaviour
    {
        public enum LoadType
        {
            Single,
            Random
        }

        [SerializeField] private LoadType loadType = LoadType.Single;

        [SerializeField, ShowIf("loadType", LoadType.Single)]
        private SessionData sessionData;

        [SerializeField, ShowIf("loadType", LoadType.Random), ListDrawerSettings(Expanded = true)]
        private SessionData[] sessionDatas;

        [SerializeField] private InteractableUnityEventWrapper interactableEvents;

        private void Start()
        {
            interactableEvents.WhenUnselect.AddListener(OnSelect);
        }

        private void OnSelect()
        {
            switch (loadType)
            {
                case LoadType.Single:
                    Session.LoadFromData(sessionData);
                    break;
                case LoadType.Random:
                    if (sessionDatas.Length > 0)
                        Session.LoadFromData(sessionDatas[Random.Range(0, sessionDatas.Length)]);
                    break;
            }
        }
    }
}
