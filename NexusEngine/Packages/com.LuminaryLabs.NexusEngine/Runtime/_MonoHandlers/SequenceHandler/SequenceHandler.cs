using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class SequenceHandler : EntitySequence<SequenceHandlerData>
    {
        public enum SequenceRunAction
        {
            RUN,
            STOP,
            FINISH,
            COMPLETE
        }

        public enum SequenceProgressionType
        {
            LINEAR,
            GROUP,
            RANDOM
        }
  

        private SequenceProgressionType progressionType;
        private SequenceRunAction runAction;
        private List<MonoSequence> targetSequences = new List<MonoSequence>();


        private List<ISequence> runningSequences = new List<ISequence>();
        protected override UniTask Initialize(SequenceHandlerData currentData)
        {
            progressionType = currentData.ProgressionType;
            runAction = currentData.RunAction;
            targetSequences = currentData.Targets;

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            await UniTask.NextFrame();
            HandleTargets();
        }

        void HandleTargets()
        {
            Nexus.Log("Hanlding Target Start: " + runAction + " with progression type: " + progressionType);
            if(runAction == SequenceRunAction.RUN)
            {   
                HandleProgressionType(RunTargetSequence);
            }
            else if(runAction == SequenceRunAction.STOP)
            {
                HandleProgressionType(StopTargetSequence);
            }
            else if(runAction == SequenceRunAction.FINISH)
            {
                HandleProgressionType(FinishTargetSequence);
            }
            else if(runAction == SequenceRunAction.COMPLETE)
            {
                HandleProgressionType(CompleteTargetSequence);
            }
        }

        void HandleProgressionType(Action<MonoSequence> HandlerFunction)
        {
            if(progressionType == SequenceProgressionType.GROUP)
            {
                HandleGroupSequences(HandlerFunction);
            }
            else if(progressionType == SequenceProgressionType.LINEAR)
            {

            }
            else if(progressionType == SequenceProgressionType.RANDOM)
            {
                HandleRandomSequence(HandlerFunction);
            }
        }

        // This method takes a function (or Action) with a parameter of type T
        void HandleRandomSequence(Action<MonoSequence> HandlerFunction)
        {
            Nexus.Log("Handling Random Sequence with Function: " + HandlerFunction.GetMethodInfo().Name );
            //get random Sequence from list and 
            MonoSequence sequence = targetSequences[UnityEngine.Random.Range(0,targetSequences.Count-1)];
            HandlerFunction(sequence);
            Complete();
        }

        void HandleGroupSequences(Action<MonoSequence> HandlerFunction)
        {
            Nexus.Log("Handling Group Sequences with Function: " + HandlerFunction.GetMethodInfo().Name + " on " + targetSequences.Count + " sequences.");
            foreach(MonoSequence sequence in targetSequences)
            {
                Nexus.Log("Handling Group Sequence: " + runAction + " with progression type: " + progressionType);
                HandlerFunction(sequence);
            }
            Complete();
        }

        void RunTargetSequence(MonoSequence sequence)
        {
            Sequence.Run(sequence, new SequenceRunData()
            {
                superSequence = this,
                onBegin = OnSequenceBegin,
                onUnload = OnSequenceUnload,
            });
        }

        async void FinishTargetSequence(MonoSequence sequence)
        {
            Nexus.Log("Finish Target Sequence" + sequence.name);
            await Sequence.Finish(sequence);
        }

        async void StopTargetSequence(MonoSequence sequence)
        {
            Nexus.Log("Stop Target Sequence: " + sequence.name);
            await Sequence.Stop(sequence);
        }

        async void CompleteTargetSequence(MonoSequence sequence)
        {
            Nexus.Log("Complete Target Sequence: " + sequence.name);
            await Sequence.Finish(sequence);
            await Sequence.Stop(sequence);
            Nexus.Log("Complete Finished on " + sequence.name);
        }

        
    

        void OnSequenceBegin(ISequence sequence)
        {
            if(runningSequences.Contains(sequence) == false)
            {
                runningSequences.Add(sequence);
            }
        }

        void OnSequenceUnload(ISequence sequence)
        {
            if(runningSequences.Contains(sequence))
            {
                runningSequences.Remove(sequence);
            }
        }

        protected override async UniTask Unload()
        {
            Nexus.Log("Complete " + name);
            while(runningSequences.Count > 0)
            {
                ISequence sequenceToStop = runningSequences[0];
                if(sequenceToStop == null) continue;
                await Sequence.Stop(sequenceToStop);
                if(runningSequences.Contains(sequenceToStop))
                {
                    runningSequences.Remove(sequenceToStop);
                }
            }
        }

        async void Complete()
        {
            if(this.phase == Phase.Run)
            {
                await Sequence.Finish(this);
                await Sequence.Stop(this);
            }
        }
    }

    [System.Serializable]
    public class SequenceHandlerData : SequenceData
    {
        public SequenceHandler.SequenceProgressionType ProgressionType;
        public SequenceHandler.SequenceRunAction RunAction;
        public List<MonoSequence> Targets;

    }
}
