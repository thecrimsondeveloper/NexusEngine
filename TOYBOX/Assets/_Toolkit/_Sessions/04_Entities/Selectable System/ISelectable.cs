using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPR
{

    public interface ISelectable
    {
        Vector3 worldPosition { get; }
        Vector3 localPosition { get; }
        void SetNormalizedProgress(float normalizedValue,SelectorBase selector);

        void Select(SelectorBase selector);
        void Deselect(SelectorBase selector);
    }

}