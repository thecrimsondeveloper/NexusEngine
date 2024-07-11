using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Sessions
{
    public class LoadSessionTriggerButton : TriggerButton
    {
        public enum LoadType
        {
            Single,
            Random
        }

        [SerializeField]
        private LoadType loadType = LoadType.Single;



        [SerializeField, ShowIf("loadType", LoadType.Single)]
        private SessionData sessionData;

        [SerializeField, ShowIf("loadType", LoadType.Random), ListDrawerSettings(Expanded = true)]
        private SessionData[] sessionDatas;
        protected override void OnClickAction()
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

        protected override void OnResetButton()
        {

        }

        protected override void OnTriggerSqueeze(float squeezeValue)
        {

        }


    }
}
