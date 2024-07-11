using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace ToyBox
{
    public class GridParent : MonoBehaviour
    {

        public List<GridCell> cells = new List<GridCell>();

        [Button]
        public async void GetAllCells()
        {
            //clear all of the cells
            cells.Clear();

            for(int i = 0; i < transform.childCount; i ++)
            {
                Transform child = transform.GetChild(i);
                cells.Add(new GridCell(child));
            }
        }

        [System.Serializable]
        public class GridCell
        {
            public GridCell(Transform cell)
            {
                cellParent = cell;
            }
            public Transform cellParent = null;

            [Button]
            public void MoveCellUp()
            {
                Vector3 startingPos = cellParent.localPosition;
                cellParent.transform.DOLocalMove(startingPos + Vector3.up, 1).OnComplete(() =>
                {
                    cellParent.transform.DOLocalMove(startingPos, 1);
                });
            }
        }
    }
}
