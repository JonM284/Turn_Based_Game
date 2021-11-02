using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project.Scripts.Level
{
    public class GridPiece : MonoBehaviour
    {
        public System.Action<Vector3> sendPosition;
       public void SetupGridPiece(System.Action<Vector3> _send)
       {
            sendPosition += _send;
       }

        public void OnMouseDown()
        {
            sendPosition?.Invoke(this.transform.position);
        }
    }
}
