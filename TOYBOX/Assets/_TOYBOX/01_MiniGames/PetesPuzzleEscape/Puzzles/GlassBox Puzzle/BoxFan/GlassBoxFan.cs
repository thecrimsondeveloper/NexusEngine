using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class GlassBoxFan : MonoBehaviour
    {
        [SerializeField] Animation anim;

        public void RunFan()
        {
            anim.Play("FanTurnOn");
        }

        public void StopFan()
        {
            anim.Play("FanTurnOff");
        }


    }
}
