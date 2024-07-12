using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Sequences
{
    [CreateAssetMenu(fileName = "SequenceLibrary", menuName = "Toolkit/Sequences/Sequence Library")]
    public class SequenceLibrary : ScriptableObject
    {

        public UnityEngine.Object[] resources;


        private void OnValidate()
        {
            for (int i = 0; i < resources.Length; i++)
            {
                //check if resource is a sequence
                if (resources[i] is GameObject obj)
                {
                    MonoBehaviour[] monos = obj.GetComponents<MonoBehaviour>();
                    for (int j = 0; j < monos.Length; j++)
                    {
                        Debug.Log(monos[j].GetType());
                        if (monos[j] is IBaseSequence)
                        {
                            Debug.Log("Sequence found");
                            resources[i] = monos[j];
                            break;
                        }
                        else
                        {
                            Debug.LogWarning("SequenceController requires a MonoBehaviour that implements ISequence.");
                            resources[i] = null;
                        }
                    }
                }
                else if (resources[i] is IBaseSequence)
                {
                    continue;
                }
                else
                {
                    Debug.LogWarning("Sequence Library requires a MonoBehaviour or Scriptable Object that implements ISequence.");
                    resources[i] = null;
                }
            }
        }

        public UnityEngine.Object Get(int index)
        {
            if (index < 0 || index >= resources.Length)
            {
                return null;
            }
            return resources[index];
        }

    }
}
