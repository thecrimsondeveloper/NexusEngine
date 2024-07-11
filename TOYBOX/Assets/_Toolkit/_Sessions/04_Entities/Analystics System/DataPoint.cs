using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPoint
{

    [SerializeField] float value = 0;
    [SerializeField] float averageValue = 0;
    [SerializeField] float avgDifference = 0;
    [SerializeField] float avgDifferenceLastUpdate = 0;
    [SerializeField] List<float> previousValues = new List<float>();

    public float Value
    {
        get => value;
        set
        {
            Update(value);
        }
    }
    public float Average { get => averageValue; }
    public float Difference { get => avgDifference; }
    public float DifferenceLastUpdate { get => avgDifferenceLastUpdate; }

    float timeLastUpdate = 0;

    public bool IsRising { get => avgDifference > 0; }
    public bool IsFalling { get => avgDifference < 0; }
    public bool HasBegunRising { get => avgDifferenceLastUpdate < 0 && avgDifference > 0; }
    public bool HasBegunFalling { get => avgDifferenceLastUpdate > 0 && avgDifference < 0; }

    protected void Update(float value)
    {
        //will use the avgDifference that was calculated last time 
        avgDifferenceLastUpdate = avgDifference;

        float deltaTime = Time.time - timeLastUpdate;
        this.value = value;
        previousValues.Add(value);

        // Remove old values
        if (previousValues.Count > 30)
        {
            previousValues.RemoveAt(0);
        }

        // Calculate average
        averageValue = 0;
        foreach (var item in previousValues)
        {
            averageValue += item;
        }
        averageValue /= previousValues.Count;

        // Calculate difference between average and current
        avgDifference = value - averageValue;

        timeLastUpdate = Time.time;
    }

}
