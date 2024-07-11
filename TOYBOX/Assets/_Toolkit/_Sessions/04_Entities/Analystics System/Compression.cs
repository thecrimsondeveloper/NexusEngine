using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace CPR.Analytics
{

    [System.Serializable]
    public class CompressionTracker
    {

        [SerializeField] List<Compression> compressions = new List<Compression>();


        [SerializeField] float currentCompressionBPM = 0;
        [SerializeField] float timeSpentCompressing = 0;
        [SerializeField] float timeAtLastCompression = 0;
        [SerializeField] float timeSinceLastCompression = 0;

        [SerializeField] float timeToExitCompression = 5;
        float timeWhenStartTracking = -1;

        public float BPM { get => currentCompressionBPM; }
        [SerializeField] bool isTracking = false;
        public bool IsTracking { get => isTracking; }
        public bool IsCompressing { get => timeSinceLastCompression < timeToExitCompression; }
        public float TimeSinceStartTracking { get => Time.time - timeWhenStartTracking; }


        public void StartTracking()
        {
            timeWhenStartTracking = Time.time;
            isTracking = true;
        }
        public void EndTracking()
        {


            isTracking = false;
            compressions.Clear();
        }


        Compression compression;
        public void StartCompression()
        {
            if (IsTracking == false)
            {
                StartTracking();
            }
            compression = new Compression(TimeSinceStartTracking, 0);
        }

        public void EndCompression()
        {
            if (IsTracking == false) return;

            compressions.Add(compression);
            timeAtLastCompression = Time.time;
            timeSpentCompressing += compression.Age;
        }

        internal void Update(DataPoint positionData)
        {
            if (IsTracking == false) return;



            timeSinceLastCompression = Time.time - timeAtLastCompression;
            if (timeSinceLastCompression > timeToExitCompression)
            {
                EndTracking();
            }

            if (compressions.Count <= 0) return;


            float trackingTime = 6;
            //remove all compressions with age over 5 seconds
            for (int i = compressions.Count - 1; i >= 0; i--)
            {
                if (compressions[i] == null)
                {
                    compressions.RemoveAt(i);
                    continue;
                }
                if (compressions[i].Age > trackingTime)
                {
                    Debug.Log("Removing compression: " + compressions[i].Age + " > " + trackingTime + "");
                    compressions.RemoveAt(i);
                }
            }

            if (compressions.Count == 0)
            {
                currentCompressionBPM = 0;
                return;
            }

            int numberOfCompressionsWithinTrackingTime = compressions.Count;
            currentCompressionBPM = compressions.Count / trackingTime * 60;
        }
    }



    [System.Serializable]
    public class Compression
    {
        public float time = 0;
        public float depth = 0;

        public float Age { get => Time.time - time; }
        public Compression(float time, float depth)
        {
            this.time = time;
            this.depth = depth;

        }
    }
}

