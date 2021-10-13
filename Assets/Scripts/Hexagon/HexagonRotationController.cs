using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Hexagon
{
    public class HexagonRotationController : MonoBehaviour
    {

        //test
        public float rotationSpeed = .2f;
        public float rotateSpeed = .2f;

        private Quaternion _targetRotation;
        private GameObject _centerObj;
        private bool _isRotating;
        

        private void Update()
        {
            if (_isRotating)
            {
                _centerObj.transform.rotation = Quaternion.Slerp(_centerObj.transform.rotation, _targetRotation, rotationSpeed);
                MapState.GameStateInfo = GameState.Rotating;

            }
           
        }

        public void RotateHexagons(List<IHexagon> selectedGroup, GameObject centerObj)
        {
            StartCoroutine(RotateHexagonsClockWise(selectedGroup, centerObj));
        }

        public IEnumerator RotateHexagonsClockWise(List<IHexagon> selectedGroup,GameObject centerObj)
        {
            for (int i = 0; i < 3; i++)
            {
                _targetRotation = Quaternion.Euler(0, 0, 120 * (i + 1));
                _centerObj = centerObj;
                _isRotating = true;
          

                if (i == 0)
                {

                    SwapHexagonsInMatris(selectedGroup[i + 1], selectedGroup[i + 2]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i + 1]);
                }
                else if (i == 1)
                {
                    SwapHexagonsInMatris(selectedGroup[i + 1], selectedGroup[0]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i + 1]);
                }
                else
                {

                    SwapHexagonsInMatris(selectedGroup[0], selectedGroup[1]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[0]);
                }




                yield return new WaitForSeconds(rotateSpeed);
            }
            _isRotating = false;
            MapState.GameStateInfo = GameState.Filled;






        }


        public void SwapHexagonsInMatris(IHexagon hex1, IHexagon hex2)
        {

            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            mapMatris[hex1.X, hex1.Y] = hex2.SelfGameObject.GetComponent<HexagonController>();

            int tempX = hex1.X;
            int tempY = hex1.Y;


            hex1.X = hex2.X;
            hex1.Y = hex2.Y;
            mapMatris[hex2.X, hex2.Y] = hex1.SelfGameObject.GetComponent<HexagonController>();
            mapMatris[hex2.X, hex2.Y].gameObject.name = "Hexagon" + " "  + hex1.X + " - " + hex1.Y;

            hex2.X = tempX;
            hex2.Y = tempY;


            mapMatris[tempX, tempY] = hex2.SelfGameObject.GetComponent<HexagonController>();

            mapMatris[tempX, tempY].gameObject.name = "Hexagon" + " " + tempX + " - " + tempY;


        }
    }
}

