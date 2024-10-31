using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class TransformModifyHandler : EntitySequence<TransformModifyData>
    {
        public enum ModifyAction
        {
            SetPosition,
            SetRotation,
            SetScale,
            MoveTo,
            RotateTo,
            ScaleTo
        }

        private ModifyAction _action;
        public List<Transform> transforms;
        public Transform target;
        public float moveSpeed = 1.0f;
        public float rotateSpeed = 1.0f;
        public float scaleSpeed = 1.0f;
        
        protected override UniTask Initialize(TransformModifyData currentData)
        {
            _action = currentData.modifyAction;
            if(currentData.transforms != null)
                transforms = currentData.transforms;
            if(currentData.target != null)
                target = currentData.target;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            switch (_action)
            {
                case ModifyAction.SetPosition:
                    foreach (var t in transforms)
                    {
                        t.position = target.position;
                    }
                    break;
                case ModifyAction.SetRotation:
                    foreach (var t in transforms)
                    {
                        t.rotation = Quaternion.Euler(target.rotation.eulerAngles);
                    }
                    break;
                case ModifyAction.SetScale:
                    foreach (var t in transforms)
                    {
                        t.localScale = target.localScale;
                    }
                    break;
                case ModifyAction.MoveTo:
                    MoveToTarget().Forget();
                    break;
                case ModifyAction.RotateTo:
                    RotateToTarget().Forget();
                    break;
                case ModifyAction.ScaleTo:
                    ScaleToTarget().Forget();
                    break;
            }
        }

        private async UniTask MoveToTarget()
        {
            foreach (var t in transforms)
            {
                while (Vector3.Distance(t.position, target.position) > 0.01f)
                {
                    t.position = Vector3.Lerp(t.position, target.position, Time.deltaTime * moveSpeed);
                    await UniTask.Yield();
                }

                Sequence.Finish(this);
                Sequence.Stop(this);
            }
        }

        private async UniTask RotateToTarget()
        {
            foreach (var t in transforms)
            {
                while (Quaternion.Angle(t.rotation, Quaternion.Euler(target.rotation.eulerAngles)) > 0.1f)
                {
                    t.rotation = Quaternion.Slerp(t.rotation, Quaternion.Euler(target.rotation.eulerAngles), Time.deltaTime * rotateSpeed);
                    await UniTask.Yield();
                }

                Sequence.Finish(this);
                Sequence.Stop(this);
            }
        }

        private async UniTask ScaleToTarget()
        {
            foreach (var t in transforms)
            {
                while (Vector3.Distance(t.localScale, target.localScale) > 0.01f)
                {
                    t.localScale = Vector3.Lerp(t.localScale, target.localScale, Time.deltaTime * scaleSpeed);
                    await UniTask.Yield();
                }

                Sequence.Finish(this);
                Sequence.Stop(this);
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class TransformModifyData : SequenceData
    {
        public TransformModifyHandler.ModifyAction modifyAction;
        public List<Transform> transforms;
        public Transform target;
    }
}
