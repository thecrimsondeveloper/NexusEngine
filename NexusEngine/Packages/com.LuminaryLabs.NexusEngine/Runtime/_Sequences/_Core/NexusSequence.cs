using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
#if ODIN_INSPECTOR

using Sirenix.OdinInspector;

#endif

using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class NexusSequence : BaseSequence<NexusSequenceData>
    {
        // private List<NexusSequenceDefinition> _sequenceDefinitions = new List<NexusSequenceDefinition>();

        [SerializeReference]
        public SequenceDefinition nexusSequence;
        public List<NexusSequenceData> beginWith = new List<NexusSequenceData>();
        public List<NexusSequenceData> waitFor = new List<NexusSequenceData>();


        ISequence runningSequence;
        List<ISequence> runningBeginningSequences = new List<ISequence>();
        List<ISequence> runningWaitForSequences = new List<ISequence>();

        protected override UniTask Initialize(NexusSequenceData currentData)
        {
            nexusSequence = currentData.nexusSequence;
            beginWith = currentData.beginWith;
            waitFor = currentData.waitFor;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            ISequence mainSequence = nexusSequence.GetSequence();
            SequenceRunData mainSequenceRunData = nexusSequence.UpdateData(new SequenceRunData{
                superSequence = this,
                onFinished = OnMainSequenceFinished
            });
            Sequence.Run(mainSequence, mainSequenceRunData);



            foreach(NexusSequenceData sequencBeginWithData in beginWith)
            {
                NexusSequence nexusSequence = new NexusSequence();
                Sequence.Run(nexusSequence, new SequenceRunData{
                    superSequence = this,
                    sequenceData = sequencBeginWithData
                });
            }

            foreach(NexusSequenceData sequencewaitForData in waitFor)
            {
                Debug.Log("RUNNING SEQUENCE: " + sequencewaitForData.GetSequence().GetType());
                NexusSequence nexusSequence = new NexusSequence();
                SequenceRunResult runResult = Sequence.Run(nexusSequence, new SequenceRunData{
                    superSequence = this,
                    sequenceData = sequencewaitForData,
                    onFinished = OnWaitForSequenceFinished
                });

                runResult.events.RegisterEvent(OnWaitForSequenceFinished, SequenceEventType.OnFinished);
            }
        }

        void OnMainSequenceFinished(ISequence sequence)
        {
            runningSequence = sequence;

        }

        async void OnWaitForSequenceFinished(ISequence sequence)
        {
            Debug.Log("SEQUENCE FINISHED FROM NEXUS SEQUENCE");
            if(runningWaitForSequences.Contains(sequence))
            {
                runningWaitForSequences.Remove(sequence);
            }


            if(runningWaitForSequences.Count == 0)
            {
                Debug.Log("Wait For Sequence Finished");
                await Sequence.Stop(runningSequence);
                await Sequence.Finish(this);
                await Sequence.Stop(this);
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class NexusSequenceData : SequenceData
    {
        [SerializeReference]
        public SequenceDefinition nexusSequence;
        public List<NexusSequenceData> beginWith = new List<NexusSequenceData>();
        public List<NexusSequenceData> waitFor = new List<NexusSequenceData>();

        public ISequence GetSequence()
        {
            return nexusSequence.GetSequence();
        }


    }




}