using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.UserInterfaces
{
    public class UserInterface : MonoSequence
    {
        [ShowInInspector]
        private readonly Dictionary<string, UserInterfacePage> pages = new Dictionary<string, UserInterfacePage>();

        protected override UniTask Finish()
        {
            // Logic for async finish
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
            // Logic for post-load operations
        }

        protected override void OnStart()
        {
            // Logic for starting the sequence
        }

        protected override void OnFinished()
        {
            // Logic for finishing the sequence
        }

        protected override void OnUnload()
        {
            // Logic for unloading the sequence
        }

        public void ShowPage(string pageName)
        {
            if (pages.TryGetValue(pageName, out var page))
            {
                Sequence.Run(page, new SequenceRunData { SuperSequence = this }).Forget();
            }
            else
            {
                Debug.LogWarning($"Page {pageName} not found.");
            }
        }

        public void HidePage(string pageName)
        {
            if (pages.TryGetValue(pageName, out var page))
            {
                Sequence.Stop(page).Forget();
            }
            else
            {
                Debug.LogWarning($"Page {pageName} not found.");
            }
        }

        public void RegisterPage(UserInterfacePage page)
        {
            string pageName = page.PageName;
            if (pages.ContainsKey(pageName))
            {
                Debug.LogWarning($"Page {pageName} already registered.");
                return;
            }
            pages.Add(pageName, page);
        }

        public void UnregisterPage(string pageName)
        {
            if (!pages.ContainsKey(pageName))
            {
                Debug.LogWarning($"Page {pageName} not registered.");
                return;
            }
            pages.Remove(pageName);
        }
    }
}
