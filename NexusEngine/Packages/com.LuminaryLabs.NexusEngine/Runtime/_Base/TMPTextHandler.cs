using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class TMPTextHandler : BaseSequence<TMPTextHandlerData>
{

    private string text;
    private float timePerCharacter;
    protected override UniTask Initialize(TMPTextHandlerData currentData)
    {
        text = currentData.text;
        timePerCharacter = currentData.timePerCharacter;
        return UniTask.CompletedTask;
    }
    protected override void OnBegin()
    {
        throw new System.NotImplementedException();
    }
}

public class TMPTextHandlerData : BaseSequenceData
{
    // public TextMeshProUGUI textMeshPro;
    public string text;
    public float timePerCharacter;
}
