using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ToyBox.Games.OpenWorld
{

    public class PlayerHead : MonoBehaviour
    {
        public List<EntityObject> lookedAtEntities = new List<EntityObject>();
        public List<EntityObject> excludedEntities = new List<EntityObject>();
        public EntityObject directEntity;
        public float meleeRange = 2f;
        public float distanceRange = 5f;

        public void Excluded(EntityObject entity)
        {
            if (excludedEntities.Contains(entity) == false)
                excludedEntities.Add(entity);
        }

        public void RemoveExclusion(EntityObject entity)
        {
            if (excludedEntities.Contains(entity))
                excludedEntities.Remove(entity);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (EntityObject entity in lookedAtEntities)
                {
                    if (entity is Item item)
                    {
                        player.Pickup(item);
                        break;
                    }
                }
            }

            //raycast
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceRange))
            {
                if (hit.collider.TryGetComponent(out EntityObject entity))
                {
                    directEntity = entity;
                }
                else
                {
                    directEntity = null;
                }
            }
            else
            {
                directEntity = null;
            }
        }


        [SerializeField] Player player;
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EntityObject item))
            {
                if (excludedEntities.Contains(item)) return;
                if (lookedAtEntities.Contains(item) == false)
                    lookedAtEntities.Add(item);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EntityObject item))
            {
                if (lookedAtEntities.Contains(item))
                    lookedAtEntities.Remove(item);
            }
        }
    }
}