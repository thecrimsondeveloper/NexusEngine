using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Users
{
    public class UserGraphicsSettings : UserSettingsSequence
    {
        public enum Resolution
        {
            High,
            Medium,
            Low
        }

        public Resolution resolution = Resolution.High;

        public override void LoadSettings()
        {
        }

        public override void SaveSettings()
        {
        }
    }
}
