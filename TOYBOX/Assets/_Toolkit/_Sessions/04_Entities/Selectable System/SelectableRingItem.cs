using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RengeGames.HealthBars;
using DG.Tweening;
using UnityEngine.PlayerLoop;


namespace CPR
{

    public class SelectableRingItem : SelectableBase
    {
        [SerializeField] Transform ringTransform;
        [SerializeField] Transform fxTransform;
        public RadialSegmentedHealthBar healthBar;

        Pose fxStartingPose; 

        private void Start() 
        {
            fxStartingPose.position = fxTransform.position;
            fxStartingPose.rotation = fxTransform.rotation;
            Reset();
        }
        
        

        void Reset()
        {
            ringTransform.localScale = Vector3.zero;
            healthBar.SetPercent(0);
            fxTransform.transform.rotation = fxStartingPose.rotation;
        }

        protected override void OnDeselect(SelectorBase selector)
        {
            Reset();
        }

        

        protected override void OnProgressChanged(float normalizedValue,SelectorBase selector)
        {

            ringTransform.localScale = Vector3.one;


            Vector3 ringHoverPoint = GetPointBetweenSelector(0.3f,selector);
            ringTransform.position = ringHoverPoint;

            ringTransform.LookAt(selector.transform.position);
            fxTransform.LookAt(selector.transform.position);

            healthBar.SetPercent(normalizedValue);
        }

        protected override void OnSelect(SelectorBase selector)
        {
            ringTransform.DOScale(0, 0.9f).SetEase(Ease.OutBounce).OnComplete(() => 
            {
                Reset();
            });
        }
    }
}
