using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Hexagon
{

    public interface IHexagon
    {
        int X { get; set; }
        int Y { get; set; }
        Color HexagonColor { get; }
        Vector2 HexagonPosition { get; }
        Vector2 HexagonStartPosition { get; }
        SpriteRenderer HexagonSpriteRenderer { get; }
        GameObject Outline { get; }
        GameObject SelfGameObject { get; }
        bool IsMoving { get; set; }
         void ChangeColor(HexagonController hexController,IHexagon hex);
    }
}
