using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class Intercom : MonoBehaviour
    {
        public AudioSource audioSource;
        public NexusCondition playerLookInvestigate = null;






        public async UniTask PlayAudio(IntercomData data)
        {
            audioSource.clip = data.intercomAudio;

            if (audioSource.clip == null)
            {
                Debug.LogError("Intercom audio clip is null");
                return;
            }


            audioSource.Play();
            await UniTask.WaitUntil(() => !audioSource.isPlaying);
        }
    }
}
