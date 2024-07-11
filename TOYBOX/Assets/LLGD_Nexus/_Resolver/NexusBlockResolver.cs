using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusResolver : MonoBehaviour
    {
        [SerializeField] NexusEntity entity;
        [SerializeField] NexusBlockDependencyDefinition blockDependencyDefinition;

        public void ResolveDependencies()
        {
            foreach (var block in blockDependencyDefinition.subBlocks)
            {
                ResolveBlock(block);
            }

            foreach (var component in blockDependencyDefinition.components)
            {
                ResolveComponent(component);
            }
        }

        public void ResolveBlock(NexusBlockDependencyDefinition block)
        {
            //instantiate block as a child of this object
            //set the block's dependencies

            foreach (var subBlock in block.subBlocks)
            {
            }
        }

        public void ResolveComponent(NexusComponentDependencyDefinition component)
        {

        }
    }
}
