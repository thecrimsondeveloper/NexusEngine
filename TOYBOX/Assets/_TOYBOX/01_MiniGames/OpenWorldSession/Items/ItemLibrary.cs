using System.Collections;
using System.Collections.Generic;
using Toolkit.Library;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{
    [CreateAssetMenu(fileName = "ItemLibrary", menuName = "ToyBox/Items/ItemLibrary")]
    public class ItemLibrary : AssetLibrary<ItemProfile>
    {
        public bool TryGetCraftableProfile(string craftID, out ItemProfile itemProfile)
        {


            Debug.Log("TryGetItemProfile: " + craftID);
            //loop through all the item profiles in the asset library and return the one that matches the craftID
            foreach (ItemProfile profile in assetProfiles)
            {
                //if the profile's craftID matches the craftID we are looking for    
                Debug.Log("Looking for: " + craftID + " Found: " + profile.CraftID);
                if (profile.CraftID == craftID)
                {
                    Debug.Log("Found: " + profile.CraftID);
                    itemProfile = profile;
                    return true;
                }
            }

            itemProfile = null;
            return false;
        }
    }
}
