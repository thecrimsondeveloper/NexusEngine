using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class NexusWeaponAttacker : MonoBehaviour
    {

        [SerializeField] NexusWeapon weapon;
        [SerializeField] PointableUnityEventWrapper pointableEvents;

        private void OnValidate()
        {
            if (weapon == null)
            {
                weapon = GetComponent<NexusWeapon>();
            }
        }

        bool selected = false;
        // Start is called before the first frame update
        void Start()
        {
            if (weapon == null)
            {
                weapon = GetComponent<NexusWeapon>();
            }

            if (pointableEvents)
            {
                pointableEvents.WhenSelect.AddListener(WhenSelect);
                pointableEvents.WhenUnselect.AddListener(WhenDeselect);
            }
        }

        private void Update()
        {
            //bool right controller trigger

            if (selected || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                Attack();
            }

        }

        [Button]
        public void Attack()
        {
            if (weapon) weapon.Use();
        }

        void WhenSelect(PointerEvent data)
        {
            selected = true;
        }

        void WhenDeselect(PointerEvent data)
        {
            selected = false;
        }
    }
}
