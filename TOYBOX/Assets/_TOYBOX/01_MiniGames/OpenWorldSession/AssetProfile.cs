using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Library
{

    public class AssetProfile<T> : AssetProfile where T : Object
    {
        [Title("Asset References")]
        [Tooltip("The asset that this profile is associated with. Prefabs, ScriptableObjects, Materials, etc.")]
        [Required]
        [SerializeField] T asset;

        public T Asset => asset;

    }

    public class AssetProfile : ScriptableObject
    {
        [Title("Asset Settings")]
        [SerializeField] string assetID;
        public string AssetID => assetID;
    }
}
