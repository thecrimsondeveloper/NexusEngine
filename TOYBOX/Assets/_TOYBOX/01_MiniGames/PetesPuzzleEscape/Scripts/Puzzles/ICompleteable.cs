using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Minigames.EscapeRoom
{
    public interface ICompleteable
    {
        void Complete()
        {
            OnComplete();
        }

        public void OnComplete()
        {
            Debug.Log("Puzzle Complete");
        }
    }
}
