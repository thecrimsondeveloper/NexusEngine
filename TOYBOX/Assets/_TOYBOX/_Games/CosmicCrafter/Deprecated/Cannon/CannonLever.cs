using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class CannonLever : MonoBehaviour
    {


        [SerializeField] Transform lever;
        [SerializeField, ReadOnly, HideInEditorMode] float angle = 0;
        [SerializeField, ReadOnly, HideInEditorMode] float angleLastFrame = 0;
        [SerializeField] float resetDelay = 1;
        [FoldoutGroup("Events")] public UnityEvent OnPulled;



        Pose levelStartPose;

        void Start()
        {
            levelStartPose = new Pose(lever.localPosition, lever.localRotation);
            angleLastFrame = lever.localEulerAngles.x;

            //invoke refresh 8 times a second
            InvokeRepeating("Refresh", 0, 0.125f);
        }

        private void Refresh()
        {
            angle = lever.localEulerAngles.x;

            CheckIsPulled();




            angleLastFrame = angle;
        }


        bool isResetting = false;

        void CheckIsPulled()
        {
            if (isResetting) return;


            //if the anglelastframe is less than 30 and the angle is greater than 30, then the lever has been pulled
            if (angleLastFrame < 25 && angle > 25)
            {
                OnPulled.Invoke();
                OnPull();
            }
        }

        async void OnPull()
        {
            isResetting = true;
            await UniTask.Delay((int)(resetDelay * 1000));
            Reset();
        }

        public void Reset()
        {

            lever.DOLocalMove(levelStartPose.position, 0.5f);
            lever.DOLocalRotateQuaternion(levelStartPose.rotation, 0.5f).OnComplete(() => isResetting = false);

        }



    }
}
