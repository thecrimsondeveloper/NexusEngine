using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Body.Input;
using Sirenix.OdinInspector;
using TMPro;
using ToyBox.Minigames.EscapeRoom;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Extras
{
    public class Keypad : MonoBehaviour
    {
        [Title("Settings")]
        public string currentCode = "";
        public int codeLength = 4;

        [Header("Dependencies")]
        [SerializeField, Required] private TMP_Text keypadDisplayText;
        [SerializeField, Required] private Renderer panelMesh;
        [SerializeField, Required] private Material panelMaterial;
        [SerializeField, Required] private AudioSource audioSource;
        [SerializeField, Required] private AudioClip buttonClickedSfx;
        [SerializeField] KeypadButton submitButton;
        [SerializeField] public List<KeypadButton> buttons = new List<KeypadButton>();



        [Title("Events")]
        public UnityEvent<string> WhenType = new UnityEvent<string>();
        public UnityEvent<string> WhenSubmit = new UnityEvent<string>();





        private void Start()
        {
            //add the click event to each button
            foreach (KeypadButton button in buttons)
            {
                button.WhenClick.AddListener(Type);
            }

            //add the click event to the submit button
            submitButton.WhenClick.AddListener(Submit);
        }

        public void Type(string value)
        {
            //add the value to the start of the code
            currentCode = value + currentCode;
            if (currentCode.Length > codeLength)
            {
                //remove the last character if the code is too long
                currentCode = currentCode.Substring(0, codeLength);
            }

            //update the display text
            keypadDisplayText.text = currentCode;

            //play the button click sound
            audioSource.PlayOneShot(buttonClickedSfx);

            //invoke the type event
            WhenType.Invoke(currentCode);
        }

        public void Type(int value)
        {
            Type(value.ToString());
        }

        public void Submit(string value)
        {
            //play the button click sound
            audioSource.PlayOneShot(buttonClickedSfx);

            //invoke the submit event
            WhenSubmit.Invoke(currentCode);
        }

        public void Clear()
        {
            currentCode = "";

            //update the display text
            keypadDisplayText.text = currentCode;
        }


#if UNITY_EDITOR
        [Button]
        public void SearchForButtons()
        {
            buttons.Clear();
            buttons.AddRange(GetComponentsInChildren<KeypadButton>());

            if (buttons.Contains(submitButton))
            {
                buttons.Remove(submitButton);
            }
        }

#endif
    }
}
