using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using ToyBox.Minigames.PetesPuzzleEscape;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class AlarmClockHandler : MonoBehaviour
    {
        [SerializeField] List<TMP_Text> minutesTexts;
        [SerializeField] List<TMP_Text> secondsTexts;
        [SerializeField] List<TMP_Text> colonTexts;

        [SerializeField] TimerLightController timerLightController;

        [SerializeField] private string alarmMinutesTarget;
        [SerializeField] private string alarmSecondsTarget;

        private string currentMinutes;
        private string currentSeconds;

        [SerializeField] private Color defaultColor;
        [SerializeField] private Color warningColor;
        [SerializeField] private Color dangerColor;

        public bool IsCountingDown => isCountingDown;

        public UnityEvent TenMinutesLeft = new UnityEvent();
        public UnityEvent FiveMinutesLeft = new UnityEvent();
        public UnityEvent OneMinuteLeft = new UnityEvent();
        public UnityEvent TimerComplete = new UnityEvent();

        public void UpdateAlarmClockTexts()
        {
            foreach (var text in minutesTexts)
            {
                text.text = currentMinutes;
            }

            foreach (var text in secondsTexts)
            {
                text.text = currentSeconds;
            }
        }

        public void UpdateTextColor()
        {
            int minutes = int.Parse(currentMinutes);
            Color targetColor;

            if (minutes >= 10)
            {
                targetColor = defaultColor;
            }
            else if (minutes >= 5)
            {
                targetColor = warningColor;
            }
            else
            {
                targetColor = dangerColor;
            }

            foreach (var text in minutesTexts)
            {
                text.faceColor = targetColor;
            }

            foreach (var text in secondsTexts)
            {
                text.faceColor = targetColor;
            }

            foreach (var text in colonTexts)
            {
                text.faceColor = targetColor;
            }
        }

        private void UpdateVFXColor()
        {
            if (currentMinutes == "10" && currentSeconds == "00")
            {
                timerLightController.SetColor(warningColor);
            }
            else if (currentMinutes == "05" && currentSeconds == "00")
            {
                timerLightController.SetColor(dangerColor);
            }
            else if (currentMinutes == "01")
            {
                float intensity = Mathf.Abs(Mathf.Sin(Time.time * 2));
                timerLightController.SetIntensity(intensity);
            }
        }

        private void Start()
        {
            alarmMinutesTarget = "00";
            alarmSecondsTarget = "00";
            currentMinutes = alarmMinutesTarget;
            currentSeconds = alarmSecondsTarget;
        }

        private bool isCountingDown = false;
        private float timer = 0f;

        private void Update()
        {
            if (isCountingDown)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    timer -= 1f;
                    UpdateTimer();
                }
            }
        }

        private void UpdateTimer()
        {
            if (int.Parse(currentSeconds) > 0)
            {
                currentSeconds = (int.Parse(currentSeconds) - 1).ToString("D2");
            }
            else if (int.Parse(currentMinutes) > 0)
            {
                currentMinutes = (int.Parse(currentMinutes) - 1).ToString("D2");
                currentSeconds = "59";
            }
            else
            {
                isCountingDown = false;
            }

            // Check if we need to trigger any events

            if (currentMinutes == "10" && currentSeconds == "00")
            {
                TenMinutesLeft.Invoke();
            }
            else if (currentMinutes == "05" && currentSeconds == "00")
            {
                FiveMinutesLeft.Invoke();
            }
            else if (currentMinutes == "01" && currentSeconds == "00")
            {
                OneMinuteLeft.Invoke();
            }
            else if (currentMinutes == "00" && currentSeconds == "00")
            {
                TimerComplete.Invoke();
            }

            //update texts and colors

            UpdateAlarmClockTexts();
            UpdateTextColor();  // Update the text color based on the remaining time
            UpdateVFXColor(); // Update the VFX color based on the remaining time
        }

        public float GetRemainingTime()
        {
            return float.Parse(currentMinutes) * 60f + float.Parse(currentSeconds);
        }

        public void SetTimer(string minutes, string seconds)
        {
            alarmMinutesTarget = minutes;
            alarmSecondsTarget = seconds;
            currentMinutes = alarmMinutesTarget;
            currentSeconds = alarmSecondsTarget;
            UpdateAlarmClockTexts();
            UpdateTextColor();  // Set initial text color
        }

        [Button("Begin Timer")]
        public void BeginTimer()
        {
            //if we are in edit mode return
            if (!Application.isPlaying)
            {
                return;
            }
            //then set the timer to start counting down
            SetTimer(alarmMinutesTarget, alarmSecondsTarget);
            timerLightController.SetColor(defaultColor);
            isCountingDown = true;
            timer = 0f;  // Reset the timer when starting
        }
    }
}
