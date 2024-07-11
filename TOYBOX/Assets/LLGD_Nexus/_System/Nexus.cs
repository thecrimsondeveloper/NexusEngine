using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class Nexus : MonoBehaviour
    {
        [ShowInInspector] static Dictionary<Guid, INexusTrackable> trackables = new Dictionary<Guid, INexusTrackable>();


        [ShowInInspector] static Dictionary<Guid, NexusObject> objects = new Dictionary<Guid, NexusObject>();
        [ShowInInspector] static Dictionary<Guid, NexusEntity> entities = new Dictionary<Guid, NexusEntity>();
        [ShowInInspector] static Dictionary<Guid, NexusBlock> blocks = new Dictionary<Guid, NexusBlock>();
        [ShowInInspector] static Dictionary<Guid, INexusConnector> connectors = new Dictionary<Guid, INexusConnector>();
        [ShowInInspector] static Dictionary<Guid, INexusConnectable> connectables = new Dictionary<Guid, INexusConnectable>();
        [ShowInInspector] static Dictionary<Guid, NexusTag> tags = new Dictionary<Guid, NexusTag>();

        public static void RegisterTrackable(INexusTrackable trackable)
        {
            //if the guid if not set, set it
            if (trackable.guid == Guid.Empty)
            {
                trackable.guid = Guid.NewGuid();
            }


            Guid guid = trackable.guid;



            if (!trackables.ContainsKey(guid))
            {
                trackables.Add(guid, trackable);
            }


            if (trackable is NexusObject obj)
            {
                if (!objects.ContainsKey(guid))
                {
                    objects.Add(guid, obj);
                }
            }

            if (trackable is NexusEntity entity)
            {
                if (!entities.ContainsKey(guid))
                {
                    entities.Add(guid, entity);
                }
            }

            if (trackable is NexusBlock block)
            {
                if (!blocks.ContainsKey(guid))
                {
                    blocks.Add(guid, block);
                }
            }

            if (trackable is NexusTag tag)
            {
                if (!tags.ContainsKey(guid))
                {
                    tags.Add(guid, tag);
                }
            }

            if (trackable is INexusConnector connector)
            {
                if (!connectors.ContainsKey(guid))
                {
                    connectors.Add(guid, connector);
                }
            }

            if (trackable is INexusConnectable connectable)
            {
                if (!connectables.ContainsKey(guid))
                {
                    connectables.Add(guid, connectable);
                }
            }
        }



        public static bool TryGetObject<T>(string guid, out T foundObject) where T : NexusObject
        {
            if (Guid.TryParse(guid, out Guid guidValue))
            {
                if (objects.TryGetValue(guidValue, out NexusObject obj))
                {
                    foundObject = (T)obj;
                    return true;
                }
            }

            foundObject = default(T);
            return false;
        }


        public static bool TryGetEntity(Guid guid, out NexusEntity entity)
        {

            entity = entities[guid];
            return entity != null;
        }


        public static bool TryGetConnectable<T>(Guid guid, out T foundConnectable) where T : INexusConnectable
        {
            if (connectables.TryGetValue(guid, out INexusConnectable connectable))
            {
                foundConnectable = (T)connectable;
                return true;
            }

            foundConnectable = default(T);
            return false;
        }
    }
}
