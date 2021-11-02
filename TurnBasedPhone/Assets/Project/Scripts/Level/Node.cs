using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project.Scripts.Level
{
    /// <summary>
    /// This code was made using the help of this tutorial
    /// https://www.youtube.com/watch?v=AKKpPmxx07w&t=446s
    /// </summary>
    public class Node
    {

        public int gridX;
        public int gridY;

        public bool InWall;

        public Vector3 nodePosition;

        public Node nodeParent;

        public int gCost;
        public int hCost;

        public int FCost { get { return gCost + hCost; } }

        public Node(bool _InWall, Vector3 _givenPosition, int _gridX, int _gridY)
        {
            InWall = _InWall;
            nodePosition = _givenPosition;
            gridX = _gridX;
            gridY = _gridY;
        }


    }
}

