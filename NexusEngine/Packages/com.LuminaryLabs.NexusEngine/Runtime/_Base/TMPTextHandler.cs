using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using TMPro;
using UnityEngine;

public class TMPTextHandler : BaseSequence<TMPTextHandlerData>
{
    // TextMeshProUGUI textMeshPro;
    private TMP_Text textToChange;
    private string text;
    private float timePerCharacter;
    protected override UniTask Initialize(TMPTextHandlerData currentData)
    {
        textToChange = currentData.textToChange;
        text = currentData.text;
        timePerCharacter = currentData.timePerCharacter;
        return UniTask.CompletedTask;
    }
    protected override async void OnBegin()
    {
        textToChange.text = ""; 
        

        //use a for loop to add each character to the text
        for (int i = 0; i < text.Length; i++)
        {
            if(Application.isPlaying == false)
            {
                return;
            }
            textToChange.text += text[i];
            await UniTask.Delay((int)(timePerCharacter * 1000));
        }

        this.Complete();
    }
}

public class TMPTextHandlerData : BaseSequenceData
{
    public TMP_Text textToChange;
    public string text;
    public float timePerCharacter;
}
