using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox
{
    [CreateAssetMenu(fileName = "GameData", menuName = "TOYBOX/GameData", order = 0)]
    public class GameData : ScriptableObject
    {
        [SerializeField] List<SessionData> sessions = new List<SessionData>();

        public List<SessionData> Sessions => sessions;
    }
}
