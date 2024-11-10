using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;



public class BaseSequenceTemplate : BaseSequence<BaseSequenceData>
{
    protected override UniTask Initialize(BaseSequenceData currentData)
    {
        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        
    }
}

public class BaseSequenceTemplateData : BaseSequenceData
{

}