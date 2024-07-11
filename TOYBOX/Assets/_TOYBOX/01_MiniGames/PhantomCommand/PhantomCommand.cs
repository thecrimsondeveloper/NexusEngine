using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.Sequences;
using System;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomCommand : MonoBehaviour, ISequence
    {
        public Guid guid { get; set; }
        public IBaseSequence superSequence
        {
            get;
            set;
        }

        public void OnSequenceLoad()
        {

        }

        public void OnSequenceFinished()
        {

        }

        public void OnSequenceStart()
        {

        }

        public void OnSequenceUnload()
        {

        }
    }
}
