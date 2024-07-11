using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace CPR
{
    [RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
    public abstract class SelectableBase : MonoBehaviour, ISelectable
    {

        [SerializeField, FoldoutGroup("Selectable")] protected Rigidbody rb;
        [SerializeField, FoldoutGroup("Selectable")] Collider col;

        [HideInInspector]public UnityEvent<SelectorBase> onSelect = new();
        [HideInInspector]public UnityEvent<SelectorBase> onDeselect = new();

        public Vector3 worldPosition => transform.position;
        public Vector3 localPosition => transform.localPosition;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            if (col == null)
            {
                col = GetComponent<Collider>();
            }
        }

#endif


        public void SetNormalizedProgress(float normalizedValue,SelectorBase selector)
        {
            OnProgressChanged(normalizedValue,selector);
        }

        public void Select(SelectorBase selector)
        {
            OnSelect(selector);
        }

        public void Deselect(SelectorBase selector)
        {
            OnDeselect(selector);
        }

        protected abstract void OnProgressChanged(float normalizedValue,SelectorBase selector);
        protected abstract void OnSelect(SelectorBase selector);
        protected abstract void OnDeselect(SelectorBase selector);

        protected Vector3 GetPointBetweenSelector(float normalizedValue, SelectorBase selectorBase)
        {
            return Vector3.Lerp(transform.position,selectorBase.transform.position, normalizedValue);
        }
    }
}
