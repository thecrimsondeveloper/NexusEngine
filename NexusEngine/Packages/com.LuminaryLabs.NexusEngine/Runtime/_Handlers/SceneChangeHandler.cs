using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuminaryLabs.NexusEngine
{
    public class SceneChangeHandler : EntitySequence<SceneChangeHandlerData>
    {
        protected override UniTask Initialize(SceneChangeHandlerData currentData)
        {
            // Initialize any setup for the scene change here if needed
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            if (currentData.asyncLoad)
            {
                // Run async scene loading
                LoadSceneAsync().Forget();
            }
            else
            {
                // Run synchronous scene loading
                LoadScene();
            }
        }

        // Method for synchronous scene loading
        private void LoadScene()
        {
            if (currentData.loadMode == SceneLoadMode.Single)
            {
                SceneManager.LoadScene(currentData.sceneName, LoadSceneMode.Single);
            }
            else if (currentData.loadMode == SceneLoadMode.Additive)
            {
                SceneManager.LoadScene(currentData.sceneName, LoadSceneMode.Additive);
            }

            Complete();
        }

        // Coroutine for asynchronous scene loading
        private async UniTaskVoid LoadSceneAsync()
        {
            AsyncOperation asyncOperation;
            if (currentData.loadMode == SceneLoadMode.Single)
            {
                asyncOperation = SceneManager.LoadSceneAsync(currentData.sceneName, LoadSceneMode.Single);
            }
            else
            {
                asyncOperation = SceneManager.LoadSceneAsync(currentData.sceneName, LoadSceneMode.Additive);
            }

            await UniTask.WaitUntil(() => asyncOperation.isDone);

            Complete();
        }

        async void Complete()
        {
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            // Handle unloading or cleanup if necessary
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class SceneChangeHandlerData : SequenceData
    {
        public string sceneName;         // The name of the scene to load
        public SceneLoadMode loadMode;   // Mode for scene loading (Single or Additive)
        public bool asyncLoad;           // Whether to load the scene asynchronously
    }

    public enum SceneLoadMode
    {
        Single,    // Load the scene and unload all others
        Additive   // Load the scene without unloading the current scene
    }
}
