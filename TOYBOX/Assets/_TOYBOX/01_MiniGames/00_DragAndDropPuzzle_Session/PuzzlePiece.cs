using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Toolkit.XR;
using ToyBox.Minigames.DragAndDropPuzzle;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace ToyBox
{
    public abstract class PuzzlePiece : MonoBehaviour
    {
        [SerializeField] protected bool canSnap = true;
        [SerializeField] protected bool isSnapped = false;
        [SerializeField] protected bool isLocked = false;
        [SerializeField] protected InteractableUnityEventWrapper events;
        [SerializeField] public PuzzleBackground background;
        [SerializeField] protected SnapPoint TargetSnapPoint { get; set; }


        protected void Start()
        {
            events.WhenUnselect.AddListener(OnUnselect);
            events.WhenSelect.AddListener(OnSelect);
        }


        protected abstract void SnapTo(SnapPoint snapPoint);
        protected abstract void OnUnselect();
        protected abstract void OnSelect();
    }
}
