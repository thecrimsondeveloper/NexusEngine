using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Toolkit.NexusEngine
{
    public abstract class NexusQueue<T> : NexusPrimitive<Queue<T>>
    {
        public void Enqueue(T item)
        {
            value.Enqueue(item);
        }

        public T Dequeue()
        {
            return value.Dequeue();
        }

        public T Peek()
        {
            return value.Peek();
        }

        public int Count => value.Count;

    }
}
