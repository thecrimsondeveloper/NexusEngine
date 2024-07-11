using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using OVR.OpenVR;
using Toolkit.XR;


namespace ToyBox.Minigames.DragAndDropPuzzle
{
    public class PuzzleBackground : MonoBehaviour
    {
        [SerializeField] private SnapPoint snapPointPrefab; // Assign a prefab for the snap point in the Inspector
        [SerializeField] private PuzzlePiece puzzlePiecePrefab;
        [SerializeField] private Texture2D puzzleTexture; // Assign a texture for the puzzle in the Inspector
        public List<SnapPoint> snapPoints = new List<SnapPoint>();
        public List<PuzzlePiece> puzzlePieces = new List<PuzzlePiece>();

        public Dictionary<SnapPoint, PuzzlePiece> correctPuzzle = new Dictionary<SnapPoint, PuzzlePiece>();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }

        private void AddToCorrectPuzzle(SnapPoint snapPoint, PuzzlePiece puzzlePiece)
        {
            correctPuzzle.Add(snapPoint, puzzlePiece);
        }

        private Vector2 gridCellSize;
        public void DivideIntoGrid(int rows, int columns)
        {
            // Get the initial position and size of the GameObject
            Vector3 initialPosition = transform.position;
            Vector3 initialScale = transform.localScale;

            // Calculate the size of each grid space
            float gridSizeX = initialScale.x / columns;
            float gridSizeY = initialScale.y / rows;

            // Calculate the offset based on the pivot being in the center
            float offsetX = -initialScale.x / 2f + gridSizeX / 2f;
            float offsetY = initialScale.y / 2f - gridSizeY / 2f;

            // Calculate the grid cell size
            gridCellSize = new Vector2(gridSizeX, gridSizeY);

            // Instantiate snap points in a grid pattern
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Calculate the position of the snap point
                    Vector3 position = new Vector3(
                        initialPosition.x + offsetX + (gridSizeX * j),
                        initialPosition.y + offsetY - (gridSizeY * i),
                        initialPosition.z);

                    // Instantiate the snap point
                    // Instantiate a snap point prefab or customize the instantiation process
                    SnapPoint snapPoint = Instantiate(snapPointPrefab, position, Quaternion.identity);
                    var snapPose = snapPoint.SnapPose;
                    snapPose.position = position;

                    // Set the rotation based on the orientation of the wall
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);  // Adjust this rotation based on your wall's orientation

                    snapPose.rotation = rotation;
                    snapPoint.SnapPose = snapPose;

                    snapPoint.transform.position = snapPoint.SnapPose.position;
                    snapPoint.transform.rotation = snapPoint.SnapPose.rotation;

                    // Add the snap point to the list of snap points
                    snapPoints.Add(snapPoint);
                    // set the size of the snap point's collider to match the grid cell size
                    snapPoint.transform.localScale = new Vector3(gridCellSize.x, gridCellSize.y, 0.3f);
                }
            }
        }

        private List<Texture2D> SplitImageIntoTextures(Texture2D originalTexture, int rows, int columns)
        {
            List<Texture2D> textures = new List<Texture2D>();

            // Calculate the size of each grid space
            int width = originalTexture.width / columns;
            int height = originalTexture.height / rows;
            // get the raw data from the original texture

            // use the raw data to create a grid of textures
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Create a new texture
                    Texture2D newTexture = new Texture2D(width, height);
                    // Set the pixels of the new texture to the pixels of the original texture in the current grid space, 
                    newTexture.SetPixels(originalTexture.GetPixels(j * width, i * height, width, height));

                    // Apply the changes to the new texture
                    newTexture.Apply();
                    // Add the new texture to the list of textures
                    textures.Add(newTexture);
                }
            }

            return textures;

        }

        //spawn puzzle pieces 
        int puzzlePieceIndex = 0;
        public void SpawnPuzzlePieces()
        {
            List<Texture2D> imageTextures = SplitImageIntoTextures(puzzleTexture, 4, 4);

            for (int i = 0; i < snapPoints.Count; i++)
            {
                Vector3 position = snapPoints[i].transform.position;

                PuzzlePiece puzzlePiece = Instantiate(puzzlePiecePrefab, position, Quaternion.identity);
                puzzlePiece.transform.localScale = new Vector3(gridCellSize.x, gridCellSize.y, 0.05f);
                puzzlePiece.background = this;

                for (int childIndex = 0; childIndex < puzzlePiece.transform.childCount; childIndex++)
                {
                    if (puzzlePiece.transform.GetChild(childIndex).CompareTag("PuzzlePieceFace"))
                        puzzlePiece.transform.GetChild(childIndex).GetComponent<MeshRenderer>().material.mainTexture = imageTextures[puzzlePieceIndex];
                }
                puzzlePieceIndex++;

                puzzlePieces.Add(puzzlePiece);
                AddToCorrectPuzzle(snapPoints[i], puzzlePiece);
            }
        }



        //break apart the puzzle pieces
        public void BreakApartPuzzle()
        {
            //tween each puzzle piece to a random position around the player
            for (int i = 0; i < puzzlePieces.Count - 1; i++)
            {
                //get a random position around the players head pose
                Vector3 randomPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                randomPosition = XRPlayer.HeadPose.position + randomPosition;
                //tween the puzzle piece to the random position
                puzzlePieces[i].transform.DOMove(randomPosition, 1f);
                //tween the puzzle piece to a rotation that looks at the player
                puzzlePieces[i].transform.DOLookAt(XRPlayer.HeadPose.position, .5f);
            }
        }
    }
}
