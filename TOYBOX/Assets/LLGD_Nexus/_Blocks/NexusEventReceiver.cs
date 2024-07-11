using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusEventReceiver : NexusPrimitive<UnityEvent>, INexusConnectable
    {
        public override UnityEvent value { get; protected set; } = new UnityEvent();


        public void AddListener(UnityAction action)
        {
            value.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            value.RemoveListener(action);
        }

        public void InvokeReceiver()
        {
            value.Invoke();
        }

        protected override void OnInitializeObject()
        {

        }


    }
}
