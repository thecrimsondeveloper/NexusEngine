using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public static class Nexus
    {
        const string LogPrefix = "(NEXUS ENGINE) ";
        
        public static void Log(string input, UnityEngine.Object context = default)
        {
            Debug.Log(LogPrefix + input, context);
        }
        public static void LogWarning(string input,UnityEngine.Object context = default)
        {
            Debug.LogWarning(LogPrefix + input);
        }
        public static void LogError(string input,UnityEngine.Object context = default)
        {
            Debug.LogWarning(LogPrefix + input, context);
        }

    
    }
}
