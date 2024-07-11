using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

using Toolkit.DependencyResolution;
using Toolkit.Playspace;
using Toolkit.XR;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UIElements;


namespace ToyBox
{
    public class Game : MonoSingleton<Game>
    {



#if UNITY_EDITOR

        [SerializeField, HideLabel]
        DebugGameInfo debugGameInfo;

        [Serializable]
        private struct DebugGameInfo
        {
            [Title("Single Dependencies"), ShowInInspector, HideLabel]
            [Tooltip("Single dependencies are dependencies that can only have one of each type, and only require one resolver.")]
            DebugDictionary<Toolkit.DependencyResolution.DependencyType, DependencyDefinition> debugDictionary
            {
                get => new DebugDictionary<DependencyType, DependencyDefinition>(DependencyManager.singleDependencies);
            }

            [Title("Dynamic Dependencies"), ShowInInspector, HideLabel, ListDrawerSettings(Expanded = true)]
            [Tooltip("Dynamic dependencies are dependencies that can have multiple of the same type, but require a different resolver for each one.")]
            List<DependencyDefinition> dynamicDependencies => DependencyManager.dynamicDependencies;

            [Title("Playspace"), ShowInInspector]
            Component playspace => Playspace.GetPlaySpace();

            [ShowInInspector]
            Vector3 playspacePosition => Playspace.pose.position;

            [ShowInInspector]
            Quaternion playspaceRotation => Playspace.pose.rotation;


            [Title("XR Player Left Hand"), ShowInInspector, HideLabel, BoxGroup("XR Player Hands")]
            DebugXRPlayerHand leftHand => new DebugXRPlayerHand(XRPlayer.LeftHand);

            [Title("XR Player Right Hand"), ShowInInspector, HideLabel, BoxGroup("XR Player Hands")]
            DebugXRPlayerHand rightHand => new DebugXRPlayerHand(XRPlayer.RightHand);



        }

        [Serializable]
        private class DebugDictionary<TKey, TValue>
        {
            [SerializeField, ListDrawerSettings(Expanded = true)]
            List<DebugDictionaryPair<TKey, TValue>> debugDictionaryPairs = new();

            //make a constructor that takes a dictionary and populates the list
            public DebugDictionary(Dictionary<TKey, TValue> dictionary)
            {
                foreach (var item in dictionary)
                {
                    DebugDictionaryPair<TKey, TValue> pair = new DebugDictionaryPair<TKey, TValue>();
                    pair.key = item.Key;
                    pair.value = item.Value;
                    debugDictionaryPairs.Add(pair);
                }
            }

            public DebugDictionary()
            {

            }

            public void Add(TKey key, TValue value)
            {
                debugDictionaryPairs.Add(new DebugDictionaryPair<TKey, TValue>(key, value));
            }
        }

        [Serializable]
        private struct DebugDictionaryPair<TKey, TValue>
        {
            [HorizontalGroup("DebugDictionaryPair"), HideLabel]
            public TKey key;
            [HorizontalGroup("DebugDictionaryPair"), HideLabel]
            public TValue value;

            //create a constructor that takes a key and a value
            public DebugDictionaryPair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        private struct DebugUnityEvent
        {
            [SerializeField, HideLabel]
            UnityEngine.Events.UnityEvent unityEvent;
        }

        [Serializable]
        private class DebugXRPlayerHand
        {
            public DebugXRPlayerHand(XRPlayer.XRPlayerHand hand)
            {
                Position = hand.Position;
                Rotation = hand.Rotation;
                fingerTipDirection = hand.FingerTipDirection;
                palmDirection = hand.PalmDirection;
                thumbDirection = hand.ThumbDirection;
                velocity = hand.velocity;
                averageVelocity = hand.averageVelocity;
                previousVelocities = hand.previousVelocities;
                speed = hand.speed;
                indexPinchStrength = hand.indexPinchStrength;
                middlePinchStrength = hand.middlePinchStrength;
                ringPinchStrength = hand.ringPinchStrength;
                pinkyPinchStrength = hand.pinkyPinchStrength;
                distanceFromNeck = hand.distanceFromNeck;
                horizontalDistanceFromNeck = hand.horizontalDistanceFromNeck;
                maxHorizontalDistance = hand.maxHorizontalDistance;
                isPunching = hand.hasResetLastPunch;
                direction = hand.direction;
            }
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 fingerTipDirection;
            public Vector3 palmDirection;
            public Vector3 thumbDirection;
            public Vector3 velocity;
            public Vector3 averageVelocity;
            public Vector3[] previousVelocities;
            public float speed;
            public float indexPinchStrength;
            public float middlePinchStrength;
            public float ringPinchStrength;
            public float pinkyPinchStrength;
            public float distanceFromNeck;
            public float horizontalDistanceFromNeck;
            public float maxHorizontalDistance;
            public bool isPunching;
            public XRPlayer.HandDirection direction;



        }
#endif
    }



}