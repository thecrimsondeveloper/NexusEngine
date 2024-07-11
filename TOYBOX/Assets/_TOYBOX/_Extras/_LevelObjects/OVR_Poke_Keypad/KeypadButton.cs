using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace ToyBox.Minigames.EscapeRoom

{
    public class KeypadButton : MonoBehaviour
    {
        [Header("Value")]
        [SerializeField] private string value;
        public UnityEvent<string> WhenClick = new UnityEvent<string>();
        public void PressButton()
        {
            WhenClick.Invoke(value);
        }
    }
}