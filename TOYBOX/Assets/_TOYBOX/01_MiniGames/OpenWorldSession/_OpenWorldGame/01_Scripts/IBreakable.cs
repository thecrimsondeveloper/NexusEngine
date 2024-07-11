using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public interface IBreakable
    {
        void OnBreak();
        void OnReset();
    }
}
