using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class AlarmClockAudio : MonoBehaviour
    {
        [SerializeField] AudioClip tenMinutesLeft;
        [SerializeField] AudioClip fiveMinutesLeft;
        [SerializeField] AudioClip oneMinuteLeft;

        [SerializeField] AudioClip tick;
        [SerializeField] AudioClip alarm;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AlarmClockHandler alarmClockHandler;

        private bool isTickingFaster = false;

        private void Start()
        {
            //double check that we have audio source
            if (audioSource == null && TryGetComponent(out AudioSource _audioSource))
            {
                this.audioSource = _audioSource;
            }

            //subscribe to alarmclockhandler events
            alarmClockHandler.TenMinutesLeft.AddListener(PlayTenMinutesLeft);
            alarmClockHandler.FiveMinutesLeft.AddListener(PlayFiveMinutesLeft);
            alarmClockHandler.OneMinuteLeft.AddListener(PlayOneMinuteLeft);
            alarmClockHandler.TimerComplete.AddListener(PlayAlarm);
        }

        public void PlayTenMinutesLeft()
        {
            audioSource.PlayOneShot(tenMinutesLeft);
        }

        public void PlayFiveMinutesLeft()
        {
            audioSource.PlayOneShot(fiveMinutesLeft);
        }

        public void PlayOneMinuteLeft()
        {
            audioSource.PlayOneShot(oneMinuteLeft);
            isTickingFaster = true;
        }

        public void PlayTick()
        {
            audioSource.PlayOneShot(tick);
        }

        public void PlayAlarm()
        {
            audioSource.PlayOneShot(alarm);
        }

        private float timeBetweenTicks = 1f;
        private float timeSinceLastTick = 0f;

        private void Update()
        {
            timeSinceLastTick += Time.deltaTime;

            if (isTickingFaster)
            {
                // Get the remaining time from the AlarmClockHandler
                float remainingTime = alarmClockHandler.GetRemainingTime();

                // Calculate the time between ticks based on remaining time
                timeBetweenTicks = Mathf.Clamp(remainingTime / 60f, 0.1f, 1f);

                if (timeSinceLastTick >= timeBetweenTicks)
                {
                    PlayTick();
                    timeSinceLastTick = 0;
                }
            }

            else if (alarmClockHandler.GetRemainingTime() != 0f)
            {
                if (timeSinceLastTick >= 1f)
                {
                    PlayTick();
                    timeSinceLastTick = 0;
                }
            }
        }
    }
}
