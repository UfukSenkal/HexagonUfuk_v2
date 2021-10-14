using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.InputData
{

    public class InputManager : MonoBehaviour
    {
        [SerializeField] InputData _inputData;
        private void Update()
        {
            if (MapState.GameStateInfo == GameState.Filled)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    _inputData.Click();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    _inputData.RotateHexagons();

                    MapState.GameStateInfo = GameState.Rotating;
                
                }
            }
         
        }

    }
}
