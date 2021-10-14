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
        
    }
}
