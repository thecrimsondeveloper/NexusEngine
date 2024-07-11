using System.Collections;
using System.Collections.Generic;
using CPR.Analytics;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VerticalMovementTracker : MonoBehaviour
{
    [SerializeField] TMP_Text direction;
    [SerializeField] Gradient speedGradient = null;


    [SerializeField] Image matchMousePos = null;
    [SerializeField] Image matchAvgPos = null;



    [Space(10)]
    [SerializeField] DataPoint positionData = new DataPoint();
    [SerializeField] CompressionTracker compressionTracker = new CompressionTracker();



#if UNITY_EDITOR

#endif

    Color targetTextColor = Color.white;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DebugCompression();
        }


        //lerp text color
        direction.color = Color.Lerp(direction.color, targetTextColor, Time.deltaTime * 5);
    }

    async void DebugCompression()
    {
        compressionTracker.StartTracking();
        await UniTask.Delay(300);
        compressionTracker.EndTracking();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetData();

        matchMousePos.rectTransform.anchoredPosition = new Vector2(matchMousePos.rectTransform.anchoredPosition.x, positionData.Value);
        matchAvgPos.rectTransform.anchoredPosition = new Vector2(matchAvgPos.rectTransform.anchoredPosition.x, positionData.Average);


        if (positionData.HasBegunFalling)
        {
            StartCompression();
        }

        if (positionData.HasBegunRising)
        {
            EndCompression();
        }



        if (compressionTracker.IsCompressing)
        {
            DuringCompressionUpdate();
        }
        else
        {
            direction.text = "Begin Compressions When Ready";
        }
    }

    void DuringCompressionUpdate()
    {
        if (compressionTracker.BPM < 100)
        {
            direction.text = "Go Faster";
            targetTextColor = speedGradient.Evaluate(0);
        }
        else if (compressionTracker.BPM > 120)
        {
            direction.text = "Go Slower";
            targetTextColor = speedGradient.Evaluate(1);
        }
        else
        {
            direction.text = "Maintain Speed";
            float ratioBetween = (compressionTracker.BPM - 100) / 20;
            Debug.Log(ratioBetween);
            targetTextColor = speedGradient.Evaluate(ratioBetween);
        }
    }

    void SetData()
    {
        positionData.Value = Input.mousePosition.y - 250;
        compressionTracker.Update(positionData);

    }





    Compression currentCompression = null;
    void StartCompression()
    {

        compressionTracker.StartCompression();
    }

    void EndCompression()
    {
        compressionTracker.EndCompression();
    }


}
