using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Sessions
{
    public class RoundBasedSessionData : SessionData
    {
        [SerializeReference] public List<RoundDefinition> rounds = new List<RoundDefinition>();

        public bool TryGetRound(int index, out RoundDefinition round)
        {
            if (index < 0 || index >= rounds.Count)
            {
                round = default;
                return false;
            }
            round = rounds[index];
            return true;
        }

    }

    [System.Serializable]
    public class RoundStats
    {
        public int roundIndex;
    }

    [System.Serializable]
    public class RoundDefinition
    {

    }
}
