using Cysharp.Threading.Tasks;

namespace LuminaryLabs.NexusEngine
{
    public class SequenceRunResult
    {
        public ISequence sequence { get; set; }
        public SequenceEvents events { get; set; }
        private UniTask task { get; set; } = default;

        public void SetTask(UniTask task)
        {
            this.task = task;
        }

        public async UniTask Async()
        {
            if (task.Status == UniTaskStatus.Pending)
                await task;
        }
 
    }
}