using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class NexusActionHandler : MonoBehaviour
    {
        enum ExecuteMode
        {
            OnAwake,
            OnStart,
            OnEnable,
            Manual
        }
        UnityEvent tryAgainTick;

        [SerializeField] ExecuteMode executeMode = ExecuteMode.OnAwake;
        [SerializeField] float delay = 0f;
        [SerializeField] bool repeatCheckIfConditionNotMet = false;


        [ShowIf(nameof(repeatCheckIfConditionNotMet)), MinValue(0.1f), SerializeField]
        float repeateCheckInterval = 1f;
        [SerializeField] NexusAction[] actions;

        private void Awake()
        {
            if (executeMode == ExecuteMode.OnAwake)
            {
                ExecuteActions();
            }
        }

        private void Start()
        {
            if (executeMode == ExecuteMode.OnStart)
            {
                ExecuteActions();
            }
        }

        private void OnEnable()
        {
            if (executeMode == ExecuteMode.OnEnable)
            {
                ExecuteActions();
            }
        }


        [SerializeField] List<NexusAction> failedActions = new List<NexusAction>();


        float lastCheckTime = 0;
        private void Update()
        {
            if (!repeatCheckIfConditionNotMet)
            {
                return;
            }

            if (failedActions.Count > 0 && Time.time - lastCheckTime > repeateCheckInterval)
            {
                lastCheckTime = Time.time;
                foreach (var action in failedActions)
                {
                    if (action.TryExecute(this))
                    {
                        failedActions.Remove(action);
                    }
                }
            }
        }

        [Button]
        public async void ExecuteActions(bool skipDelay = false)
        {

            if (!skipDelay)
            {
                await UniTask.Delay((int)(delay * 1000));
            }

            foreach (var action in actions)
            {
                if (action.TryExecute(this))
                {

                }
                else
                {
                    if (repeatCheckIfConditionNotMet) failedActions.Add(action);
                }
            }
        }
    }
}
