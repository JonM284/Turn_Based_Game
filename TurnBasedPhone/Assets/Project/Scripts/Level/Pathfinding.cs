using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Level
{
    //code made using the help of https://www.youtube.com/watch?v=AKKpPmxx07w
    public class Pathfinding : MonoBehaviour
    {

        public A_Grid grid;
        public Transform startPos;
        public Transform endPos;


        // Start is called before the first frame update
        void Start()
        {
            grid = GetComponent<A_Grid>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                FindPath(startPos.position, endPos.position);
            }
        }

        public void SetStartPosition(Transform _start)
        {
            startPos = _start;
        }

        public void SetEndPosition(Transform _end)
        {
            endPos = _end;
        }

        void FindPath(Vector3 _startPos, Vector3 _endPos)
        {
            Node startNode = grid.GridFromWorldPosition(_startPos);
            Node desiredNode = grid.GridFromWorldPosition(_endPos);

            List<Node> OpenList = new List<Node>();
            HashSet<Node> ClosedList = new HashSet<Node>();

            OpenList.Add(startNode);

            while (OpenList.Count > 0)
            {
                Node currentNode = OpenList[0];
                for (int i = 1; i < OpenList.Count; i++)
                {
                    if (OpenList[i].FCost < currentNode.FCost || OpenList[i].FCost == currentNode.FCost && OpenList[i].hCost < currentNode.hCost)
                    {
                        currentNode = OpenList[i];
                    }
                }
                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);

                if (currentNode == desiredNode)
                {
                    GetFinalPath(startNode, desiredNode);
                }

                foreach (Node neighborNode in grid.GetNeighboringNodes(currentNode))
                {
                    if (!neighborNode.InWall || ClosedList.Contains(neighborNode))
                    {
                        continue;
                    }
                    int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighborNode);
                    if (moveCost < neighborNode.gCost || !OpenList.Contains(neighborNode))
                    {
                        neighborNode.gCost = moveCost;
                        neighborNode.hCost = GetManhattenDistance(neighborNode, desiredNode);
                        neighborNode.nodeParent = currentNode;
                        if (!OpenList.Contains(neighborNode))
                        {
                            OpenList.Add(neighborNode);
                        }
                    }
                }

            }

        }

        void GetFinalPath(Node _startingNode, Node _finalNode)
        {
            List<Node> _finalPath = new List<Node>();
            Node _currentNode = _finalNode;

            while (_currentNode != _startingNode)
            {
                _finalPath.Add(_currentNode);
                _currentNode = _currentNode.nodeParent;
            }

            _finalPath.Reverse();

            grid.finalPath = _finalPath;

        }

        int GetManhattenDistance(Node _nodeA, Node _nodeB)
        {
            int x = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
            int y = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

            return x + y;
        }
    }
}

