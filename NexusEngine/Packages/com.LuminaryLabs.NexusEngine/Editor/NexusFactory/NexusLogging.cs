using System.Collections;
using System.Collections.Generic;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class NexusLogging : NexusFactoryPane
{

    public NexusLogging()
    {
        title = "Logging";
    }
    protected override void WhenDraw()
    {
        if (GUILayout.Button("Dump Log"))
        {
            DumpLog();
        }
    }

    private void DumpLog()
    {
        // Implement the functionality to dump or display logs here
        Nexus.LogDump();
    }
}

