using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public static class Nexus
    {
        const string LogPrefix = "(NEXUS ENGINE) ";

        static string logDump;
        
        public static void Log(string input, UnityEngine.Object context = default)
        {   
            string log = LogPrefix + input;
            logDump += log + "\n";
            Debug.Log(log, context);
        }
        public static void LogWarning(string input,UnityEngine.Object context = default)
        {
            Debug.LogWarning(LogPrefix + input);
        }
        public static void LogError(string input,UnityEngine.Object context = default)
        {
            Debug.LogWarning(LogPrefix + input, context);
        }

        public static void Log(object obj)
        {
            Type objType = obj.GetType();
            string logMessage = $"-------------------------------------\n";
            logMessage += "Object: {objType.Name}\n";

            // Access both public and private properties (with BindingFlags)
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                try
                {
                    // Get the value of the property, even if it is private
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
                    // Get the value of the field, even if it is private
                    object value = field.GetValue(obj);
                    logMessage += $"{field.Name} (Field): {value}\n";
                }
                catch (Exception e)
                {
                    logMessage += $"Could not log field '{field.Name}' due to: {e.Message}\n";
                }
            }

            // Log the entire message in one Debug.Log call
            Nexus.Log(logMessage);
        }

        public static void LogDump()
        {
            Debug.Log(LogPrefix + logDump);
        }

        
    }


    
}

