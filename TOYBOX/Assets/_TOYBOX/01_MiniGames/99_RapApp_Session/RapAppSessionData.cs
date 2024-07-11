using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox.Minigames.RapApp
{
    [CreateAssetMenu(menuName = "TOYBOX/Sessions/Rap App Session Data")]
    public class RapAppSessionData : RoundBasedSessionData
    {
        public List<WordGroup> wordGroups = new List<WordGroup>();
    }

    [System.Serializable]
    public struct WordGroup
    {
        public string[] words;
    }
}

