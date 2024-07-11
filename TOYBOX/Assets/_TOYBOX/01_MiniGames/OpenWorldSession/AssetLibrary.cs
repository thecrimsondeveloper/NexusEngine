using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Library
{
    [CreateAssetMenu(fileName = "AssetLibrary", menuName = "Toolkit/AssetLibrary")]
    public class AssetLibrary : ScriptableObject
    {
        public List<AssetProfile> assetProfiles = new List<AssetProfile>();


        public AssetProfile GetAssetProfile(string assetID)
        {
            foreach (AssetProfile assetProfile in assetProfiles)
            {
                if (assetProfile.AssetID == assetID)
                {
                    return assetProfile;
                }
            }
            return null;
        }

    }


    public abstract class AssetLibrary<T> : ScriptableObject where T : AssetProfile
    {
        public List<AssetProfile> assetProfiles = new List<AssetProfile>();
        public T GetAssetProfile(string assetID)
        {
            foreach (T assetProfile in assetProfiles)
            {
                if (assetProfile.AssetID == assetID)
                {
                    return assetProfile;
                }
            }
            return null;
        }
    }
}
