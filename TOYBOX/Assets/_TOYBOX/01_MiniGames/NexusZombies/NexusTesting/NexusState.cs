using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusState : MonoBehaviour
    {

        [ShowInInspector] private static NexusGameState state;

        public static void SaveState()
        {
            // string json = 
        }

        public static void SetState<T>(T newState = null) where T : NexusGameState
        {
            Debug.Log("Setting State: " + newState != null);
            state = newState != null ? newState : new NexusGameState() as T;
        }


        public static T As<T>() where T : NexusGameState
        {
            if (state is T t)
            {
                return t;
            }
            return null;
        }




    }

    [System.Serializable]
    public class NexusGameState
    {

    }


}
