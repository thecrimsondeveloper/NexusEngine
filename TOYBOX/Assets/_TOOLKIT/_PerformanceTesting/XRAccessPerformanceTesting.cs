using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using TMPro;
using Toolkit.XR;
using Unity.VisualScripting;
using UnityEngine;

namespace Toolkit.Testing
{
    public class XRAccessPerformanceTesting : MonoBehaviour
    {
        public int deltaMS = 1000;
        public int FPS = 60;
        public int iterations = 1000;

        [SerializeField] TMP_Text iteractionsText = null;
        [SerializeField] TMP_Text runTimeText = null;
        [SerializeField] PointableUnityEventWrapper dividerButton = null;
        [SerializeField] PointableUnityEventWrapper multiplierButton = null;
        [SerializeField] PointableUnityEventWrapper runButton = null;

        Vector3 dividerStartLocalPos;
        Vector3 multiplierStartLocalPos;
        Vector3 runStartLocalPos;

        private void Start()
        {
            iteractionsText.text = iterations.ToString();
            dividerButton.WhenSelect.AddListener(DivideIterations);
            multiplierButton.WhenSelect.AddListener(MultiplyItations);
            runButton.WhenSelect.AddListener(BeginTest);

            dividerStartLocalPos = dividerButton.transform.localPosition;
            multiplierStartLocalPos = multiplierButton.transform.localPosition;
            runStartLocalPos = runButton.transform.localPosition;
        }

        private void Update()
        {
            deltaMS = (int)(Time.deltaTime * 1000);
            FPS = (int)(1f / Time.deltaTime);



        }

        [Button]
        private async void BeginTest(PointerEvent evt)
        {
            StartCoroutine(StartTest());

            runButton.gameObject.SetActive(false);
            await UniTask.Delay(100);

            runButton.transform.localPosition = runStartLocalPos;
            runButton.gameObject.SetActive(true);
        }


        private IEnumerator StartTest()
        {
            yield return new WaitForSeconds(0.1f);

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            for (int i = 0; i < iterations; i++)
            {
                // var xr = UnityEngine.XR.XRDevice.isPresent;
                Vector3 pos = XRPlayspace.Info.RandomWallPosition;
            }

            sw.Stop();
            Debug.Log($"Time taken for {iterations} iterations: {sw.ElapsedMilliseconds}ms");
            runTimeText.text = sw.ElapsedMilliseconds.ToString() + "ms";


        }

        public async void MultiplyItations(PointerEvent evt)
        {
            iterations *= 10;

            //with comma
            iteractionsText.text = iterations.ToString("N0");

            multiplierButton.gameObject.SetActive(false);

            //set the 
            await UniTask.Delay(100);

            multiplierButton.gameObject.SetActive(true);
            //set position
            multiplierButton.transform.localPosition = multiplierStartLocalPos;
            runTimeText.text = "";
        }

        public async void DivideIterations(PointerEvent evt)
        {
            iterations /= 10;
            iteractionsText.text = iterations.ToString("N0");

            dividerButton.gameObject.SetActive(false);

            //set the
            await UniTask.Delay(100);

            dividerButton.gameObject.SetActive(true);

            //set position
            dividerButton.transform.localPosition = dividerStartLocalPos;

            runTimeText.text = "";
        }


    }
}
