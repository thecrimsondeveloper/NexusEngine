using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.Users
{
    public class UserSettings : UserSettingsSequence
    {
        [Title("User Settings")]
        [SerializeField] private string userName;
        [SerializeField] private string userAge;
        [SerializeField] private UserGraphicsSettings userGraphicsSettings;
        [SerializeField] private UserAccessibilitySettings userAccessibilitySettings;
        [SerializeField] private UserAudioSettings userAudioSettings;
        [SerializeField] private UserInputSettings userInputSettings;
        [SerializeField] private UserGameplaySettings userGameplaySettings;

        //on load the sequence
        protected override void OnLoadSequence()
        {
            SequenceRunData runData = new SequenceRunData();

            Sequence.Run(userGraphicsSettings);
            Sequence.Run(userAccessibilitySettings);
            Sequence.Run(userAudioSettings);
            Sequence.Run(userInputSettings);
            Sequence.Run(userGameplaySettings);
        }

        public override void LoadSettings()
        {
        }

        public override void SaveSettings()
        {
        }
    }

}
