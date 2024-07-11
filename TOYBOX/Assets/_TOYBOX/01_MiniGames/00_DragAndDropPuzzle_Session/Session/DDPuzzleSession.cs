using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sessions;
using Toolkit.XR;
using UnityEngine;
using Extras;
using PositionHandling;

namespace ToyBox.Minigames.DragAndDropPuzzle
{
    public class DDPuzzleSession : Session
    {
        [SerializeField] DDPuzzleSessionData sessionData;

        [SerializeField] PointPositionSettings positionSetter;
        [SerializeField] PuzzleBackground background;
        public override SessionData SessionData { get => sessionData; set => sessionData = (DDPuzzleSessionData)value; }
        public override void OnSessionStart()
        {
            // Vector2 wallDimensions = XRPlayspace.WorkingWall.GetComponent<OVRScenePlane>().Dimensions;
            // background.transform.localScale = new Vector3(wallDimensions.x, wallDimensions.y, 0.2f);
            //load the puzzle
            LoadPuzzle().Forget();
        }

        protected async UniTask LoadPuzzle()
        {
            background.DivideIntoGrid(4, 4);
            await UniTask.Delay(2000);
            background.SpawnPuzzlePieces();
            await UniTask.Delay(2000);
            background.BreakApartPuzzle();
        }

        public override void OnSessionEnd()
        {

        }

        public override UniTask OnLoad()
        {
            // XRPlayspace.Target = XRPlayspace.XRPlayspaceTarget.WALL;
            return UniTask.CompletedTask;
        }

        public override UniTask OnUnload()
        {
            return UniTask.CompletedTask;
        }

        private void Update()
        {
            //get the dimensions of the wall and set the background to match
            //set the background to match the wall
            //set the background to match the wall
            // background.transform.position = XRPlayspace.WorkingWall.transform.position;
            // background.transform.rotation = XRPlayspace.WorkingWall.transform.rotation;
        }
    }
}
