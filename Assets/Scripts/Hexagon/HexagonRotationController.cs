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
        private bool _isRotating;
        

        private void Update()
        {

           

                if (_isRotating)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed);
                    
                    

                    if (Quaternion.Angle(transform.rotation, _targetRotation) <= 2f)
                    {
                        _isRotating = false;
                       MapState.GameStateInfo = GameState.Filled;

                    }
                    else
                    {
                        MapState.GameStateInfo = GameState.Rotating;
                    }
                }
            

           
        }

        public void RotateHexagons(List<IHexagon> selectedGroup)
        {
            StartCoroutine(RotateHexagonsClockWise(selectedGroup));
        }

        public IEnumerator RotateHexagonsClockWise(List<IHexagon> selectedGroup)
        {
            
            for (int i = 0; i < 3; i++)
            {

                if (MapState.GameStateInfo == GameState.Explode)
                {
                    break;
                }
                _targetRotation = Quaternion.Euler(0, 0, 120 * (i + 1));
                _isRotating = true;
                yield return new WaitForSeconds(rotateSpeed);
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
                
                


                yield return new WaitForSeconds(rotationSpeed);
                
            }

            MapState.GameStateInfo = GameState.Moving;


        }
        public IEnumerator RotateHexagonsOppositeClockWise(List<IHexagon> selectedGroup)
        {

           


            for (int i = selectedGroup.Count - 1; i >= 0; i--)
            {

                if (MapState.GameStateInfo == GameState.Explode)
                {
                    break;
                }

                _targetRotation = Quaternion.Euler(0, 0, 120 * (i + 1));
                if (i == 0)
                {

                    SwapHexagonsInMatris(selectedGroup[2], selectedGroup[1]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[2]);
                }
                else if (i == 1)
                {
                    SwapHexagonsInMatris(selectedGroup[i - 1], selectedGroup[2]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i - 1]);
                }
                else
                {

                    SwapHexagonsInMatris(selectedGroup[i - 1], selectedGroup[i - 2]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i - 1]);
                }

                MapState.GameStateInfo = GameState.Rotating;
                _isRotating = true;


                yield return new WaitForSeconds(rotateSpeed);

            }

            MapState.GameStateInfo = GameState.Moving;




        }

        public void SwapHexagonsInMatris(IHexagon hex1, IHexagon hex2)
        {

            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            mapMatris[hex1.X, hex1.Y] = hex2.SelfGameObject.GetComponent<HexagonController>();

            int tempX = hex1.X;
            int tempY = hex1.Y;


            hex1.X = hex2.X;
            hex1.Y = hex2.Y;
            hex1.SelfGameObject.GetComponent<HexagonController>().X = hex1.X;
            hex1.SelfGameObject.GetComponent<HexagonController>().Y = hex1.Y;
            mapMatris[hex2.X, hex2.Y] = hex1.SelfGameObject.GetComponent<HexagonController>();
            mapMatris[hex2.X, hex2.Y].gameObject.name = "Hexagon" + " "  + hex1.X + " - " + hex1.Y + "Rotated";

            hex2.X = tempX;
            hex2.Y = tempY;
            hex2.SelfGameObject.GetComponent<HexagonController>().X = tempX;
            hex2.SelfGameObject.GetComponent<HexagonController>().Y = tempY;

            mapMatris[tempX, tempY] = hex2.SelfGameObject.GetComponent<HexagonController>();

            mapMatris[tempX, tempY].gameObject.name = "Hexagon" + " " + tempX + " - " + tempY + "Rotated";


        }
    }
}

