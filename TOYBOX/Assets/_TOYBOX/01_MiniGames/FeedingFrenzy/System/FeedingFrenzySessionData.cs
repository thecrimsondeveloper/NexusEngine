using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox.Minigames.FeedingFrenzy
{
    [CreateAssetMenu(menuName = "TOYBOX/Sessions/Feeding Frenzy Session Data")]
    public class FeedingFrenzySessionData : SessionData
    {
        public enum PlayerMode
        {
            Hippo,
            Soldier
        }

        public PlayerMode playerMode = PlayerMode.Hippo;
        public GameObject hippoPlayerPrefab;
        public GameObject soldierPlayerPrefab;
    }
}

