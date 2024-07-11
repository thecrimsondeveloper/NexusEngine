using System.Collections;
using System.Collections.Generic;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;

namespace Toolkit.DependencyResolution
{
    public class EntityResolver : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public static void ResolveDependencies(ResolutionData resolutionData)
        {
            Debug.Log("Resolving dependencies for " + resolutionData.gameObject.name);
            // Resolve all dependencies on entity
            if (resolutionData.gameObject && resolutionData.resolutionDefinition)
            {
                //set name
                resolutionData.gameObject.name = resolutionData.resolutionDefinition.Name;
            }

        }


    }

    public struct ResolutionData
    {
        public IEntity entity;
        public GameObject gameObject;
        public EntityResolutionDefinition resolutionDefinition;
    }
}
