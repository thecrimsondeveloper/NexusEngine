using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusMin : NexusFloat
    {
        public List<NexusFloat> inputs;
        public NexusFloat ouput;

        public override float value
        {
            get
            {
                float min = float.MaxValue;
                foreach (NexusFloat input in inputs)
                {
                    if (input.value < min)
                    {
                        min = input.value;
                    }
                }
                ouput.Set(min);
                return min;
            }
            protected set
            {
                base.value = value;
            }
        }
    }
}
