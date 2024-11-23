using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;


namespace  LuminaryLabs.NexusEngine.UnityHandlers
{
    

    /// <summary>
    /// Handler for performing actions on a GameObject.
    /// </summary>
    public class BaseGameObjectHandler : BaseSequence<BaseGameObjectHandlerData>
    {
        // Private fields to store data from the BaseGameObjectHandlerData.
        private GameObject _targetGameObject;
        private GameObjectAction _action;

        /// <summary>
        /// Initializes the sequence with the provided data.
        /// </summary>
        /// <param name="currentData">The current data for the sequence.</param>
        /// <returns>A UniTask representing the initialization process.</returns>
        protected override UniTask Initialize(BaseGameObjectHandlerData currentData)
        {
            if (currentData == null)
            {
                Debug.LogError("BaseGameObjectHandler: Initialize failed. No data provided.");
                return UniTask.CompletedTask;
            }

            // Assign private fields from data.
            _targetGameObject = currentData.TargetGameObject;
            _action = currentData.Action;

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called when the sequence begins.
        /// </summary>
        protected override void OnBegin()
        {
            if (_targetGameObject == null)
            {
                Debug.LogWarning("BaseGameObjectHandler: No target GameObject set.");
                return;
            }

            switch (_action)
            {
                case GameObjectAction.Activate:
                    _targetGameObject.SetActive(true);
                    break;

                case GameObjectAction.Deactivate:
                    _targetGameObject.SetActive(false);
                    break;

                case GameObjectAction.Destroy:
                    Object.Destroy(_targetGameObject);
                    break;

                case GameObjectAction.ToggleActiveState:
                    _targetGameObject.SetActive(!_targetGameObject.activeSelf);
                    break;

                default:
                    Debug.LogWarning("BaseGameObjectHandler: Unsupported action.");
                    break;
            }

            this.Complete();
        }
    }

    /// <summary>
    /// Data class for BaseGameObjectHandler.
    /// </summary>
    public class BaseGameObjectHandlerData : BaseSequenceData
    {
        /// <summary>
        /// The target GameObject to perform the action on.
        /// </summary>
        public GameObject TargetGameObject;

        /// <summary>
        /// The action to perform on the target GameObject.
        /// </summary>
        public GameObjectAction Action;
    }

    /// <summary>
    /// Enum representing actions to perform on a GameObject.
    /// </summary>
    public enum GameObjectAction
    {
        Activate,
        Deactivate,
        Destroy,
        ToggleActiveState
    }

}