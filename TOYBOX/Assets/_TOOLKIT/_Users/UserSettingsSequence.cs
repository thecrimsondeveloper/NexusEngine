using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.Users
{
    public abstract class UserSettingsSequence : ScriptableSequence
    {
        protected override void AfterLoad()
        {
            // Load previous settings
            LoadSettings();
            OnLoadSequence();
        }

        protected virtual void OnLoadSequence() { }

        protected override void OnStart()
        {
            OnStartSequence();
        }

        protected virtual void OnStartSequence() { }

        protected override void OnFinished()
        {
            OnFinishSequence();
        }

        protected virtual void OnFinishSequence() { }

        protected override void OnUnload()
        {
            // Save settings
            SaveSettings();
            OnUnloadSequence();
        }

        protected virtual void OnUnloadSequence() { }

        protected override UniTask Finish()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        // Abstract load settings
        public abstract void LoadSettings();

        // Abstract save settings
        public abstract void SaveSettings();
    }
}
