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
            if (Input.GetMouseButtonDown(0) && MapState.GameStateInfo == GameState.Filled)
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
