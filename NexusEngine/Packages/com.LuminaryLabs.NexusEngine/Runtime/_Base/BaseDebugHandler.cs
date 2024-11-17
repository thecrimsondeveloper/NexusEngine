using System;
using System.Text;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

/// <summary>
/// Handles logging and outputting debug information.
/// </summary>
public class BaseDebugHandler : BaseSequence<BaseDebugHandlerData>
{
    private string message;
    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(BaseDebugHandlerData currentData)
    {
        message = currentData.message;
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
        if (currentData != null)
        {
            // Create a temporary log class, populate it, and generate the log
            TempLogData tempLogData = new TempLogData
            {
                Message = message,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            // Generate and output the log
            string log = GenerateLog(tempLogData);
            Debug.Log(log);
        }
        else
        {
            Debug.LogWarning("No data provided for BaseDebugHandler.");
        }

        Complete();
    }

    /// <summary>
    /// Generates a log string from the temporary log data.
    /// </summary>
    /// <param name="data">The temporary log data.</param>
    /// <returns>A formatted string representing the log data.</returns>
    private string GenerateLog(TempLogData data)
    {
        StringBuilder logBuilder = new StringBuilder();
        logBuilder.AppendLine("--- Log Output ---");
        logBuilder.AppendLine($"Message: {data.Message}");
        logBuilder.AppendLine($"Timestamp: {data.Timestamp}");
        logBuilder.AppendLine("------------------");
        return logBuilder.ToString();
    }

    /// <summary>
    /// Temporary class to hold log data.
    /// </summary>
    private class TempLogData
    {
        public string Message;
        public string Timestamp;
    }
}

/// <summary>
/// Contains data for the debug handler.
/// </summary>
[Serializable]
public class BaseDebugHandlerData : BaseSequenceData
{
    public string message = "Default debug message";
}
