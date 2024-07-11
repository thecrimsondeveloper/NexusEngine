using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Style
{
    public interface IStyleable
    {
    }

    public interface IStyleable<T> : IStyleable where T : StyleProfile
    {
        T StyleProfile { get; set; }



        void SetStyle(T styleProfile)
        {
            StyleProfile = styleProfile;
            ApplyStyle(styleProfile);
        }

        UniTask ApplyStyle(T styleProfile);
    }
}
