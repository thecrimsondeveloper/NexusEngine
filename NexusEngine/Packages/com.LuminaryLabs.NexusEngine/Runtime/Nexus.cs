using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LuminaryLabs.NexusEngine
{
    public static class Nexus
    {
        const string LogPrefix = "(NEXUS ENGINE) ";
        public static List<string> logDump = new List<string>();


        public static void Log(string input, UnityEngine.Object context = default)
        {
            string log = LogPrefix + input;
            logDump.Add(log);
            Debug.Log(log, context);
        }

        public static void LogWarning(string input, UnityEngine.Object context = default)
        {
            Debug.LogWarning(LogPrefix + input, context);
        }

        public static void LogError(string input, UnityEngine.Object context = default)
        {
            Debug.LogError(LogPrefix + input, context);
        }

        public static void Log(object obj)
        {
            Type objType = obj.GetType();
            string logMessage = $"-------------------------------------\n";
            logMessage += $"Object: {objType.Name}\n";

            // Access both public and private properties
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                try
                {
                    object value = property.GetValue(obj, null);
                    logMessage += $"({property.Name}): {value}\n";
                }
                catch (Exception e)
                {
                    logMessage += $"Failed To Log Property: '{property.Name}' due to: {e.Message}\n";
                }
            }

            // Access both public and private fields
            FieldInfo[] fields = objType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    object value = field.GetValue(obj);
                    logMessage += $"{field.Name} (Field): {value}\n";
                }
                catch (Exception e)
                {
                    logMessage += $"Could not log field '{field.Name}' due to: {e.Message}\n";
                }
            }

            Log(logMessage);
        }

        public static void LogDump()
        {
#if UNITY_EDITOR
            // Open a save file dialog to let the user select a file location
            string path = EditorUtility.SaveFilePanel("Save Log Dump", "", "NexusLogDump.json", "json");

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning(LogPrefix + "Log dump canceled by the user.");
                return;
            }

            try
            {
                // Serialize the logDump list to JSON format
                string json = JsonUtility.ToJson(new LogDumpWrapper { logs = logDump }, true);

                // Write the JSON content to the selected file
                File.WriteAllText(path, json);
                Debug.Log(LogPrefix + "Log dump successfully saved to: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError(LogPrefix + "Failed to save log dump: " + e.Message);
            }
#endif
        }

        // Wrapper class for JSON serialization
        [Serializable]
        private class LogDumpWrapper
        {
            public List<string> logs;
        }
    }
}
