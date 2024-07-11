using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using Toolkit.XR;

namespace ToyBox
{
    public class GenericPuzzlePiece : PuzzlePiece
    {
        protected override void OnUnselect()
        {
            //if the puzzle piece is further than 0.1 units from a snap point then return
            foreach (SnapPoint snapPoint in background.snapPoints)
            {
                if (Vector3.Distance(transform.position, snapPoint.transform.position) < 0.15f)
                {
                    SnapTo(snapPoint);
                    return;
                }
            }
        }

        protected override void OnSelect()
        {
            //if the puzzle piece is snapped to a snap point then return
            if (isSnapped)
            {
                //set the snap point to unoccupied so that more pieces can be placed there
                TargetSnapPoint.IsOccupied = false;
                isSnapped = false;
                return;
            }
        }

        protected override void SnapTo(SnapPoint snapPoint)
        {
            TargetSnapPoint = snapPoint;
            if (snapPoint.IsOccupied)
            {
                FindClosestSnapPoint();
                return;
            }

            Pose snapPose = snapPoint.SnapPose;
            transform.DOMove(snapPose.position, 0.5f);
            transform.DORotateQuaternion(snapPose.rotation, 0.5f);
            snapPoint.IsOccupied = true;
            isSnapped = true;
        }

        private void FindClosestSnapPoint()
        {
            //get the closest snap point to this puzzle piece that is not occupied
            float minDistance = float.MaxValue;
            SnapPoint closestSnapPoint = null;
            foreach (SnapPoint snapPoint in background.snapPoints)
            {
                if (!snapPoint.IsOccupied)
                {
                    float distance = Vector3.Distance(transform.position, snapPoint.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestSnapPoint = snapPoint;
                    }
                }
            }
            if (closestSnapPoint != null)
            {
                SnapTo(closestSnapPoint);
            }
        }

        protected void Update()
        {
            if (adjustingPosition) return;
            if (isSnapped) return;
            CheckProximityToPlayer();
        }

        float distanceToPlayer;
        bool adjustingPosition = false;
        protected void CheckProximityToPlayer()
        {
            adjustingPosition = true;
            if (isSnapped) return;
            if (Vector3.Distance(transform.position, XRPlayer.HeadPose.position) < 0.05f)
            {
                MoveAwayFromPlayer();
            }
        }

        protected void MoveAwayFromPlayer()
        {
            //tween away from the player while distance to player is less than 0.05f
            transform.DOMove(transform.position + (transform.position - XRPlayer.HeadPose.position).normalized * 0.1f, 0.2f).SetEase(Ease.OutBack).OnComplete(() => adjustingPosition = false);
        }
    }
}
