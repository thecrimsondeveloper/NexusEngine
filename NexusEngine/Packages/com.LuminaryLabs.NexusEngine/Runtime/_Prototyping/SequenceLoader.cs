using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Sequences
{
    public class SequenceLoader : MonoBehaviour
    {
        [SerializeField] private SequenceLibrary sequenceLibrary;
        [SerializeField] private SequenceState initialState;

        public List<Object> loadedSequences = new List<Object>();
        public string debugInfo;

        private StringBuilder debugStringBuilder = new StringBuilder();

        [Button]
        public void Load()
        {
            // Clear the list of loaded sequences
            loadedSequences.Clear();
            debugStringBuilder.Clear();

            SequenceRunData runData = new SequenceRunData
            {
                InitializationData = sequenceLibrary
            };


            if (initialState != null)
            {
                Sequence.Run(initialState, runData);
            }
            else
            {
                debugStringBuilder.AppendLine("Initial state is not set.");
            }

            debugInfo = debugStringBuilder.ToString();
        }




    }
}
