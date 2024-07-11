using System;
using Sirenix.OdinInspector;

namespace Toolkit.Extras
{
    [Serializable]
    public class NumberRange
    {
        [HorizontalGroup("FRange"), HideLabel] public float a;
        [HorizontalGroup("FRange"), HideLabel] public float b;

        //working variables to save on garbage collection
        static float workingValue = 0;
        static Random randomEngine = new();

        public NumberRange(float min, float max)
        {
            this.a = min;
            this.b = max;
        }

        public NumberRange Negative()
        {
            return new NumberRange(-a, -b);
        }

        public float Clamp(float value)
        {
            //if the value is greater than the max, return the max
            if (value > b)
            {
                return b;
            }

            //if the value is less than the min, return the min
            if (value < a)
            {
                return a;
            }

            //otherwise return the value
            return value;
        }


        public float Random()
        {
            workingValue = (float)randomEngine.NextDouble() * (b - a) + a;
            return workingValue;
        }

        //<summary>
        //Lerp between the min and max values using the input t
        //</summary>
        public float Lerp(float t)
        {
            if (t > 1)
            {
                return b;
            }
            else if (t < 0)
            {
                return a;
            }
            return t * (b - a) + a;
        }


        //<summary>
        //InverseLerp between the min and max values using the input value
        //</summary>
        public float InverseLerp(float value)
        {
            return (value - a) / (b - a);
        }
        public bool IsInRange(float value)
        {
            return value >= a && value <= b;
        }

        public static implicit operator float(NumberRange minMax)
        {
            return minMax.b;
        }
        public static implicit operator NumberRange(float value)
        {
            return new NumberRange(0, value);
        }
    }
}
