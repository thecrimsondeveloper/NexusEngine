using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sessions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.UserInterface
{

    public class UI : MonoBehaviour
    {
        public enum UIState { LAUNCHER, LOADING, PLAYING, PAUSE, END }

        [Title("UI Settings")]
        [SerializeField, OnValueChanged(nameof(RefreshState))] UIState state = UIState.LAUNCHER;

        [Title("UI Pages")]
        [SerializeField] LauncherMenu launcherMenu;
        [SerializeField] PauseMenu pauseMenu;
        [SerializeField] LoadingUI loadingUI;
        [SerializeField] SessionEndMenu sessionEndMenu;
        [SerializeField] SessionPlayingMenu sessionPlayingMenu;

        Page currentPage = null;

        bool uiRefreshing = false;

        [Button]
        public async UniTask SetState(UIState newState, bool force = false)
        {
            if (state == newState && !force) return;
            state = newState;

            await RefreshState();
        }

        async UniTask RefreshState()
        {
            if (uiRefreshing) return;
            uiRefreshing = true;

            //await cl
            if (currentPage != null)
                await currentPage.SetIsOpen(false);

            await UniTask.NextFrame();

            //set the current page based on the state
            switch (state)
            {
                case UIState.LAUNCHER:
                    currentPage = launcherMenu;
                    break;
                case UIState.LOADING:
                    currentPage = loadingUI;
                    break;
                case UIState.PLAYING:
                    currentPage = sessionPlayingMenu;
                    break;
                case UIState.PAUSE:
                    currentPage = pauseMenu;
                    break;
                case UIState.END:
                    currentPage = sessionEndMenu;
                    break;
            }

            await currentPage.SetIsOpen(true);
            await UniTask.NextFrame();

            uiRefreshing = false;
        }




        private void OnValidate()
        {
            if (launcherMenu == null) launcherMenu = GetComponentInChildren<LauncherMenu>();
            if (pauseMenu == null) pauseMenu = GetComponentInChildren<PauseMenu>();
            if (loadingUI == null) loadingUI = GetComponentInChildren<LoadingUI>();
            if (sessionEndMenu == null) sessionEndMenu = GetComponentInChildren<SessionEndMenu>();
            if (sessionPlayingMenu == null) sessionPlayingMenu = GetComponentInChildren<SessionPlayingMenu>();
        }



        void Start()
        {
            Session.OnSessionLoad.AddListener(OnSessionLoad);
            Session.OnSessionUnloaded.AddListener(OnSessionUnloaded);
            Session.OnSessionLoaded.AddListener(OnSessionLoaded);
            Session.OnSessionUnload.AddListener(OnSessionUnload);

            RefreshState();
        }

        private void OnSessionLoad(SessionData sessionData)
        {
            Debug.Log("Session Loaded");
            SetState(UIState.LOADING);
        }

        private void OnSessionLoaded(SessionData sessionData)
        {
            SetState(UIState.PLAYING);
        }

        private void OnSessionUnload()
        {
            SetState(UIState.LOADING);
        }

        private void OnSessionUnloaded()
        {
            SetState(UIState.LAUNCHER);
        }

        public void SetScore(float newScore)
        {

        }



    }
}