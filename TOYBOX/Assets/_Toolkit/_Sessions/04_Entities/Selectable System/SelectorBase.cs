using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace CPR
{
    public abstract class SelectorBase : MonoBehaviour
    {

        [SerializeField, FoldoutGroup("Selector Base")] protected List<TrackedSelectable> hoveredSelectables;
        [SerializeField, FoldoutGroup("Selector Base")] protected CapsuleCollider selector;
        [SerializeField, FoldoutGroup("Selector Base")] float range = 5;
        [SerializeField, FoldoutGroup("Selector Base")] float gazeWidth = 1;

        public void RemoveSelectable(TrackedSelectable trackedSelectable)
        {
            if (hoveredSelectables.Contains(trackedSelectable))
            {
                hoveredSelectables.Remove(trackedSelectable);
            }
        }

        public float Range => range;
        public float Width => gazeWidth;
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            Refresh(range,gazeWidth);
        }
#endif

        private void Update()
        {
            for (int i = 0; i < hoveredSelectables.Count; i++)
            {
                TrackedSelectable trackedSelectable = hoveredSelectables[i];
                trackedSelectable.Update();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ISelectable selectable))
            {
                TrackedSelectable newTrackedSelectable = new TrackedSelectable(this,selectable,3, other);
                hoveredSelectables.Add(newTrackedSelectable);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            foreach (TrackedSelectable trackedSelectable in hoveredSelectables)
            {
                if (trackedSelectable.col == other)
                {
                    trackedSelectable.selectable.Deselect(this);
                    hoveredSelectables.Remove(trackedSelectable);
                    return;
                }
            }
        }







        [Button, FoldoutGroup("Selector Base")]
        void Refresh(float viewRange, float width)
        {
            if (selector == null)
            {
                selector = GetComponent<CapsuleCollider>();
            }
            if (selector == null)
            {
                selector = gameObject.AddComponent<CapsuleCollider>();
            }

            if (selector == null)
            {
                return;
            }

            if (selector.isTrigger == false)
                selector.isTrigger = true;

            selector.direction = 2;

            Vector3 center = selector.center;
            center.z = viewRange / 2;

            selector.center = center;

            selector.height = viewRange;
            selector.radius = width;
        }
    }

    [System.Serializable]
    public class TrackedSelectable
    {

        public SelectorBase selector;
        public ISelectable selectable;
        public float timeUntilSelect = 3;
        public Collider col;

        [ShowInInspector]float timer = 0;

        internal TrackedSelectable(SelectorBase selector, ISelectable selectable, float timeUntilSelect, Collider col)
        {
            this.selector = selector;
            this.selectable = selectable;
            this.timeUntilSelect = timeUntilSelect;
            this.col = col;
        }

        public void Update()
        {
            timer += Time.deltaTime;

            float progress = timer / timeUntilSelect;
            selectable.SetNormalizedProgress(progress,selector);

            if (progress > 1)
            {
                selectable.Select(selector);
                selector.RemoveSelectable(this);
                return;
            }
        }
    }
}
