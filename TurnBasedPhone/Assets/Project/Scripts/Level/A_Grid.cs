using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Level
{
    public class A_Grid : MonoBehaviour
    {
        //code made using the help of https://www.youtube.com/watch?v=AKKpPmxx07w
        public Transform startPos;
        public LayerMask wallMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        public float distance;

        Node[,] grid;
        public List<Node> finalPath;

        float nodeDiameter;
        int gridSizeX, gridSizeY;



        // Start is called before the first frame update
        void Start()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
            GenerateGrid();
        }

        void GenerateGrid()
        {
            grid = new Node[gridSizeX,gridSizeY];
            Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool wall = true;
                    if(Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                    {
                        wall = false;
                    }
                    grid[x, y] = new Node(wall, worldPoint, x, y);
                }
            }
        }

        public Node GridFromWorldPosition(Vector3 _position)
        {
            Debug.Log($"<color=red>Incoming position: {_position}</color>");
            float xpoint = (_position.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float ypoint = (_position.z - transform.position.z + gridWorldSize.y / 2) / gridWorldSize.y;

            xpoint = Mathf.Clamp01(xpoint);
            ypoint = Mathf.Clamp01(ypoint);

            int x = Mathf.CeilToInt((gridSizeX) * xpoint) - 1;
            int y = Mathf.CeilToInt((gridSizeY) * ypoint) - 1;

            x = Mathf.Clamp(x, 0, gridSizeX - 1);
            y = Mathf.Clamp(y, 0, gridSizeY - 1);

            Debug.Log($"<color=orange>Values: {x}, {y} ; GridNode:{grid[x,y].nodePosition} Points: {xpoint}, {ypoint}</color>");
            return grid[x,y];
        }

        public List<Node> GetNeighboringNodes(Node _node)
        {
            List<Node> neighboringNodes = new List<Node>();

            int xCheck;
            int yCheck;

            xCheck = _node.gridX + 1;
            yCheck = _node.gridY;
            if (xCheck > 0 && xCheck < gridSizeX)
            {
                if (yCheck > 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck,yCheck]);
                }
            }

            xCheck = _node.gridX - 1;
            yCheck = _node.gridY;
            if (xCheck > 0 && xCheck < gridSizeX)
            {
                if (yCheck > 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }

            xCheck = _node.gridX;
            yCheck = _node.gridY + 1;
            if (xCheck > 0 && xCheck < gridSizeX)
            {
                if (yCheck > 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }

            xCheck = _node.gridX;
            yCheck = _node.gridY - 1;
            if (xCheck > 0 && xCheck < gridSizeX)
            {
                if (yCheck > 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }
            return neighboringNodes;

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                Node playerNode = GridFromWorldPosition(startPos.position);
                foreach (Node node in grid)
                {
                    if (node.InWall)
                    {
                        Gizmos.color = Color.cyan;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }

                    if (finalPath != null && finalPath.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }

                    if (playerNode.nodePosition == node.nodePosition)
                        Gizmos.color = Color.yellow;

                    Gizmos.DrawWireCube(node.nodePosition, Vector3.one * (nodeDiameter - distance));
                }
            }
        }

    }
}

