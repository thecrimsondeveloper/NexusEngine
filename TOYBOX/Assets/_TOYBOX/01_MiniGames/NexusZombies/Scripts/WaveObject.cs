using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Extras
{
    public class WaveObject : MonoBehaviour
    {

        public Wave parentWave;
        public UnityEvent WhenDestroyed = new UnityEvent();

        public void Initialize(Wave wave)
        {
            this.parentWave = wave;
        }

        private void OnDestroy()
        {
            WhenDestroyed.Invoke();
        }
    }
}
