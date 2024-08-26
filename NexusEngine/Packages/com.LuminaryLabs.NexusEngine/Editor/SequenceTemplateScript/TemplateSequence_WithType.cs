using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace YourNamespace
{
    public class TemplateClass : TemplateBaseClass<TemplateClassData>
    {

        #region SubSequences
        #endregion


        #region SubSequenceData
        #endregion


        #region Sequence Logic
        protected override UniTask Initialize(TemplateClassData currentData)
        {

            // Initialization logic here
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            #region Template Sequence Run
            // Run a sequence
            ISequence sequence = null;
            object data = null;
            Sequence.Run(sequence, new SequenceRunData
            {
                superSequence = this,
                sequenceData = data,
                onFinished = TemplateFinished
            });
            #endregion
            // Logic to execute when the sequence begins
        }

        protected override UniTask Unload()
        {
            // Cleanup logic here
            return UniTask.CompletedTask;
        }

        #endregion



        #region SequenceFinishFunctions 
        void TemplateFinished(ISequence sequence)
        {

        }
        #endregion
    }

    #region Data Class

    [System.Serializable]
    public class TemplateClassData
    {

        #region MonoSequnces

        #endregion

        #region ScriptableSequences

        #endregion

        #region Data Fields

        #endregion

        // Define data fields here
    }

    #endregion
}
