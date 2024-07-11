using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.Users
{
    [CreateAssetMenu(fileName = "New User Session", menuName = "Toolkit/Users/User Session")]
    public class UserSession : ScriptableSequence
    {
        [SerializeField] DateTime sessionStart;
        [SerializeField] DateTime sessionEnd;
        [SerializeReference]
        public List<UserAction> actionsTaken = new List<UserAction>();

        // Register action
        public void RegisterAction(UserAction action)
        {
            if (action == null)
            {
                Debug.LogWarning("UserSession: RegisterAction: action is null");
                return;
            }

            if (actionsTaken.Contains(action))
            {
                Debug.LogWarning("UserSession: RegisterAction: action already registered");
                return;
            }
            actionsTaken.Add(action);
        }

        protected override UniTask Finish()
        {
            sessionEnd = DateTime.Now;
            // Save session logic here
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            // Logic for async load
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            // Logic for async unload
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            // Logic to execute after load
        }

        protected override void OnStart()
        {
            sessionStart = DateTime.Now;
        }

        protected override void OnFinished()
        {
            sessionEnd = DateTime.Now;
        }

        protected override void OnUnload()
        {
            // Logic to execute on sequence unload
        }
    }
}
