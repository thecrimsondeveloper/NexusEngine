using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class TriggerDetector : NexusBlock
    {
        List<NexusEntity> entities = new List<NexusEntity>();
        private void OnTriggerEnter(Collider other)
        {
            NexusEntity entity = other.GetComponent<NexusEntity>();
            if (entity != null)
            {
                entities.Add(entity);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            NexusEntity entity = other.GetComponent<NexusEntity>();
            if (entity != null)
            {
                entities.Remove(entity);
            }
        }

        public void ForEach(UnityAction<NexusEntity> action)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] == null)
                {
                    entities.RemoveAt(i);
                    i--;
                    continue;
                }
                action.Invoke(entities[i]);
            }
        }

    }
}
