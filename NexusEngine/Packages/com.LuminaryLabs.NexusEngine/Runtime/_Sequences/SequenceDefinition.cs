
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{

    [System.Serializable]
    public class BaseSequenceDefinition
    {
        [SerializeReference]
        public BaseSequence sequenceToRun;
        [SerializeReference]
        public BaseSequenceData sequenceData;
    }







}