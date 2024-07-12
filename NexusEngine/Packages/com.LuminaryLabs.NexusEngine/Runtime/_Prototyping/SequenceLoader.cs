
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Sequences
{
    public class SequenceLoader : MonoBehaviour
    {
        [SerializeField] private SequenceLibrary sequenceLibrary;
        [SerializeField] private SequenceState initialState;

        public List<Object> loadedSequences = new List<Object>();

        [Button]
        public void Load()
        {
            if (initialState != null)
            {
                LoadSequence(initialState);
            }
            else
            {
                Debug.LogWarning("Initial state is not set.");
            }
        }

        private IBaseSequence LoadSequence(SequenceState state)
        {
            UnityEngine.Object obj = sequenceLibrary.Get(state.ID);

            if (obj == null)
            {
                Debug.LogWarning("Sequence not found in library.");
                return null;
            }

            // Create the sequence
            IBaseSequence sequence = LoadObject(obj);

            if (sequence == null)
            {
                Debug.LogWarning("Failed to load sequence.");
                return null;
            }

            // Track the loaded sequence
            loadedSequences.Add(sequence as Object);

            // Get all properties of type UnityEngine.Object
            PropertyInfo[] properties = sequence.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            List<PropertyInfo> objectProperties = new List<PropertyInfo>();

            foreach (PropertyInfo property in properties)
            {

                if (property.PropertyType.IsSubclassOf(typeof(IBaseSequence)))
                {
                    Debug.Log($"Property {property.Name} is a sequence.");
                    objectProperties.Add(property);
                }
                else
                {
                    Debug.Log($"Property {property.Name} is not a sequence. and is of type {property.PropertyType}");
                }
            }

            // Load and assign sub-sequences
            for (int i = 0; i < state.SubSequenceIDs.Length; i++)
            {
                SequenceState subState = state.SubSequenceIDs[i];
                IBaseSequence subSequence = LoadSequence(subState);
                if (subSequence != null)
                {
                    // Set the property based on the index
                    if (i < objectProperties.Count)
                    {
                        objectProperties[i].SetValue(sequence, subSequence as UnityEngine.Object);
                    }
                    else
                    {
                        Debug.LogWarning($"No property available to assign sub-sequence at index {i}.");
                    }
                }
            }

            return sequence;
        }

        private IBaseSequence LoadObject(UnityEngine.Object obj)
        {
            if (obj is MonoBehaviour mono)
            {
                MonoBehaviour spawned = Instantiate(mono);
                if (spawned is IBaseSequence sequence)
                {
                    return sequence;
                }
            }
            else if (obj is IBaseSequence sequence)
            {
                return sequence;
            }

            return null;
        }
    }
}
