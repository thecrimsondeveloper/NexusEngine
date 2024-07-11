using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace ToyBox
{
    public class CollectiveDOTweenAnimationEvents : MonoBehaviour
    {
        [SerializeField] float timeBewteenEachMultiplier = 0.1f;
        [SerializeField] float timeCap = 0.5f;
        [SerializeField] Transform[] targets;
        [SerializeField] Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();
        [SerializeField] bool shouldUseOriginalScale;
        [SerializeField] Ease ease = Ease.Linear;

        public void SetScaleToAllTargets(float scale)
        {
            foreach (Transform target in targets)
            {
                target.localScale = Vector3.one * scale;
            }
        }

        public void SetScaleToZeroOnAllTargets()
        {
            foreach (Transform target in targets)
            {
                if (originalScales.ContainsKey(target) == false)
                {
                    originalScales.Add(target, target.localScale);
                }

                target.localScale = Vector3.zero;
            }
        }

        public async void RandomScaleInTargets(float timeRangeFromOne)
        {
            float min = timeCap - timeRangeFromOne;
            min = min < 0 ? 0 : min;
            float max = timeCap + timeRangeFromOne;
            float scaleTime = Random.Range(min, max);

            foreach (Transform target in targets)
            {
                ScaleInTransform(target, scaleTime);

                //provide some buffer time for the next scale for performance reasons
                //we don't want a too many targets scaling at once
                await UniTask.Delay((int)(1000 * scaleTime * timeBewteenEachMultiplier));
                scaleTime = Random.Range(min, max);
            }
        }

        public async void RandomScaleTargetsToZero(float timeRangeFromOne)
        {
            Debug.Log("RandomScaleTargetsToZero");
            float min = 1 - timeRangeFromOne;
            float max = 1 + timeRangeFromOne;
            float scaleTime = Random.Range(min, max);

            for (int i = 0; i < targets.Length; i++)
            {
                Transform target = targets[i];

                ScaleOutTransform(target, scaleTime);
                Debug.Log("ScaleOutTransform: " + i + " " + targets.Length);
                //provide some buffer time for the next scale for performance reasons
                //we don't want a too many targets scaling at once
                await UniTask.Delay(50);
                scaleTime = Random.Range(min, max);
            }

        }

        void ScaleInTransform(Transform target, float duration)
        {
            if (shouldUseOriginalScale)
            {
                target.DOScale(originalScales[target], duration).SetEase(ease);
            }
            else
            {
                target.DOScale(Vector3.one, duration).SetEase(ease);
            }
        }

        void ScaleOutTransform(Transform target, float duration)
        {
            target.DOScale(Vector3.zero, duration);
        }
    }
}
