using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Users
{
    public class UserAccessibilitySettings : UserSettingsSequence
    {
        public bool isColorBlindModeEnabled = false;

        [SerializeField, ShowIf(nameof(isColorBlindModeEnabled)), EnumToggleButtons, LabelText("Color Blind Mode")]
        public ColorBlindMode colorBlindMode = ColorBlindMode.Off;
        public enum ColorBlindMode
        {
            Off,
            Protanopia,
            Deuteranopia,
            Tritanopia
        }

        public override void LoadSettings()
        {
        }

        public override void SaveSettings()
        {
        }
    }
}
