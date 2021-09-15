using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Level
{
    public class GridGeneration : MonoBehaviour
    {

        [SerializeField] private Vector2Int gridAmount;
        [SerializeField] private float gridPieceSize;

        [SerializeField] private GameObject gridFloorPrefab, gridOffsetFloorPrefab;

        [SerializeField] private Material offsetMaterial;

        public GameObject[,] gridObjects;

        public int MiddleX { get; set; }

        public enum GenerationState
        {
            START,
            GENERATING,
            FINISHED
        }

        public GenerationState genState = GenerationState.START;

        public void GenerateGrid()
        {
            genState = GenerationState.GENERATING;
            gridObjects = new GameObject[gridAmount.x,gridAmount.y];
            MiddleX = gridAmount.x / 2;
            for (int x = 0; x < gridAmount.x; x++)
            {
                for (int y = 0; y < gridAmount.y; y++)
                {
                    float xPos = transform.position.x + x * gridPieceSize;
                    float yPos = transform.position.z + y * gridPieceSize;

                    Vector3 spawnLoc = new Vector3(xPos, transform.position.y, yPos);
                    GameObject currentGridPiece = null;
                    if ((x % 2 == 0 && y % 2 != 1))
                    {
                        currentGridPiece = Instantiate(gridOffsetFloorPrefab, spawnLoc, Quaternion.identity);
                    }else if (x % 2 != 0 && y % 2 != 0)
                    {
                        currentGridPiece = Instantiate(gridOffsetFloorPrefab, spawnLoc, Quaternion.identity);
                    }
                    else
                    {
                        currentGridPiece = Instantiate(gridFloorPrefab, spawnLoc, Quaternion.identity);
                    }
                    currentGridPiece.transform.parent = this.transform;

                    gridObjects[x, y] = currentGridPiece;
                }
            }
            genState = GenerationState.FINISHED;
        }

    }
}

