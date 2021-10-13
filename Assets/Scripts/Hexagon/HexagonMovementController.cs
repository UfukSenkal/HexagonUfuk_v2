using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Hexagon
{
    public class HexagonMovementController : MonoBehaviour
    {
        public void Move(Vector2 lerpPos, float speed)
        {
            transform.position = Vector3.Lerp(transform.position, lerpPos, Time.deltaTime * speed);
        }
        
        public void MoveAroundCircle(Vector2 startPos, Vector2 nextPos, Vector3 center)
        {
            transform.position = Vector3.Slerp(startPos, nextPos, Time.deltaTime * 2f);
            transform.position += center;
        }
    }
}
