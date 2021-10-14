using HexagonDemo.Match;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonDemo.Hexagon
{
    public class HexagonRotationController : MonoBehaviour
    {

        //test
        public float rotationSpeed = .2f;
        public float rotateSpeed = .2f;

        private Quaternion _targetRotation;

        private List<IHexagon> _selectedGroup;



        public void RotateHexagons(List<IHexagon> selectedGroup)
        {
            _selectedGroup = new List<IHexagon>(selectedGroup);
            
            StartCoroutine(RotateHexagonsClockWise(selectedGroup));

        }

        public IEnumerator RotateHexagonsClockWise(List<IHexagon> selectedGroup)
        {


            int angleMultiplier = 0;

            for (int i = selectedGroup.Count - 1; i >= 0; i--)
            {
                angleMultiplier++;
                if (MapState.GameStateInfo == GameState.Explode)
                {
                    break;
                }
                _targetRotation = Quaternion.Euler(0, 0, -120 * angleMultiplier);
                MapState.GameStateInfo = GameState.Rotating;
                yield return new WaitForSeconds(rotateSpeed);


                while (Quaternion.Angle(transform.rotation, _targetRotation) >= 2f)
                {
                     transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotateSpeed);
                    yield return null;
                    
                   
                }

                SwapHexagonsInMatris(_selectedGroup[0], _selectedGroup[1]);
                SwapHexagonsInMatris(_selectedGroup[0], _selectedGroup[2]);

                MapState.GameStateInfo = GameState.Filled;
                MatchManager.Instance.FindNewNeighbours();
                MatchManager.Instance.CheckMatchForMap(true);



                yield return new WaitForSeconds(rotationSpeed);
            }

            




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

