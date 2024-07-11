using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusList<T> : NexusPrimitive<List<T>>
    {

        public NexusBool purgeOnGet;

        [SerializeField] List<T> _value;
        public override List<T> value
        {
            get
            {
                if (purgeOnGet)
                {
                    PurgeNullValues();
                }

                return _value;
            }
            protected set => _value = value;
        }

        public void PurgeNullValues()
        {
            //loop through each value and if it is null remove it
            for (int i = 0; i < _value.Count; i++)
            {
                if (_value[i] == null)
                {
                    Debug.Log("PURGE: TRUE");
                    _value.RemoveAt(i);
                    i--;
                }
                else
                {
                    Debug.Log($"NOT PURGE ({i}): FALSE");
                }
            }
        }


        protected override void OnInitializeObject()
        {
            base.OnInitializeObject();

            if (purgeOnGet == null)
            {
                purgeOnGet = CreateInstance<NexusBool>();
                purgeOnGet.Set(false);
            }

            purgeOnGet.InitializeObject();
        }



        public void Add(T item)
        {
            _value.Add(item);
        }

        public void SafeAdd(T item)
        {
            if (!_value.Contains(item))
            {
                _value.Add(item);
            }
        }

        public void Remove(T item)
        {
            _value.Remove(item);
        }

        public void SafeRemove(T item)
        {
            if (_value.Contains(item))
            {
                _value.Remove(item);
            }
        }

    }
}
