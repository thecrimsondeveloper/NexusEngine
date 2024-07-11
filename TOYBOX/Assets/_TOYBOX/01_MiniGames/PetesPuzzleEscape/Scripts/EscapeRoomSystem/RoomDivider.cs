using Toolkit.XR;
using ToyBox.Minigames.EscapeRoom;
using UnityEngine;

namespace ToyBox
{
    public class RoomDivider : MonoBehaviour
    {
        [SerializeField] private Puzzle openPuzzle;
        [SerializeField] private Puzzle spawnedPuzzle;
        [SerializeField] private float puzzleMovementSpeed = 1.5f;
        [SerializeField] private float interactionThreshold = 1.5f;



        private void Start()
        {
            //subscribe to the OnPuzzleComplete event
            // openPuzzle.OnPuzzleComplete.AddListener(OpenDivider);
            spawnedPuzzle = Instantiate(openPuzzle, transform.position, Quaternion.identity);
        }

        private void FollowPlayer()
        {
            Vector3 localX = transform.right;
            localX.y = 0;
            localX.Normalize();

            float spawnedPuzzleX = spawnedPuzzle.transform.position.x;
            float puzzleZDifferenceFromPlayer = XRPlayer.HeadPose.position.x - spawnedPuzzleX;

            //move the puzzle to the player's z position along the localZ axis
            spawnedPuzzle.transform.position = Vector3.Lerp(spawnedPuzzle.transform.position, transform.position + localX * puzzleZDifferenceFromPlayer, puzzleMovementSpeed * Time.deltaTime);

            //rotate the puzzle to face the player
            spawnedPuzzle.transform.LookAt(XRPlayer.HeadPose.position);
        }
    }
}
