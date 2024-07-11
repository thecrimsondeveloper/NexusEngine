using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;


[System.Serializable]
public class GameScene
{

#if UNITY_EDITOR
    [SerializeField, OnValueChanged(nameof(OnSceneChanged))] SceneAsset scene = null;
    void OnSceneChanged()
    {
        sceneName = scene.name;
    }

    [OnValueChanged(nameof(OnSceneChanged))]
#endif
    [SerializeField] string sceneName = null;

    public string SceneName => sceneName;

    [SerializeField] LoadSceneMode loadMode = LoadSceneMode.Additive;
    [ShowInInspector] public bool IsSceneLoaded => SceneManager.GetSceneByName(sceneName).isLoaded;

    public void LoadScene(bool forceReload = false)
    {
        //if the scene is already loaded, don't try to load it again
        if (SceneManager.GetSceneByName(sceneName).isLoaded == true && forceReload == false)
        {
            return;
        }

        SceneManager.LoadSceneAsync(sceneName, loadMode);
    }

    public void UnloadScene()
    {
        //if the scene is not loaded, don't try to unload it
        if (SceneManager.GetSceneByName(sceneName).isLoaded == false)
        {
            return;
        }
        SceneManager.UnloadSceneAsync(sceneName);
    }


}
