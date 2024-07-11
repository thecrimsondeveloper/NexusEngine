using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace Toolkit.Entity
{
    public interface IPlayer : ICharacter
    {
        List<NexusInventory> inventories { get; set; }

    }
}
