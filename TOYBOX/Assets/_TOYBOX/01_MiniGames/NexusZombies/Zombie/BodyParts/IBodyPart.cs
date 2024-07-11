using System.Collections;
using System.Collections.Generic;
using Toolkit.Entity;
using UnityEngine;

namespace Toolkit.Extras
{
    public interface IBodyPart
    {
        IAttackable parentAttackable { get; set; }
    }
}
