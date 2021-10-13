using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Match
{

    public class MatchManager : MonoBehaviour
    {
        [SerializeField] InputData.InputData _inputData;
        private void Update()
        {
            if (MapState.GameStateInfo == GameState.Rotating)
            {
                FindNewNeighbours();
            }

            if (MapState.GameStateInfo == GameState.Filled)
            {
                CheckMatch();
            }
        }

        private void CheckMatch()
        {
            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            foreach (var hexagon in mapMatris)
            {


                hexagon.InstantiatedNeighbourData.FindNeighbours();
                foreach (var _neighbourList in hexagon.InstantiatedNeighbourData.SelectableHexagonList)
                {
                    if (_neighbourList.Count >= 3)
                    {

                        if (_neighbourList.FindAll(p => p.HexagonColor == _neighbourList[0].HexagonColor).Count == _neighbourList.Count)
                        {
                            MapState.GameStateInfo = GameState.Explode;
                            StartCoroutine(DestroyNeighbourList(mapMatris, _neighbourList));
                            //if (bombHexagon != null)
                            //{
                            //    bombTime--;
                            //    bombHexagon.GetComponent<Hexagon.HexagonController>().SetBombText(bombTime.ToString());
                            //}
                            //int score = (_neighbourList.Count * scoreController.ScoreMult);

                            //scoreController.Score += score;
                            //scoreController.ScoreTextUpdate();
                            break;
                        }
                    }
                }
                if (MapState.GameStateInfo == GameState.Explode)
                {
                    break;
                }
                
            }
        }

        private IEnumerator DestroyNeighbourList(Hexagon.HexagonController[,] mapMatris, List<Hexagon.IHexagon> _neighbourList)
        {
                _inputData.ClearLastSelection();
            yield return new WaitForSeconds(.5f);
            foreach (var item in _neighbourList)
            {
                //mapMatris[item.X, item.Y] = null;
                Destroy(item.SelfGameObject);
            }
        }

        void FindNewNeighbours()
        {
            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            foreach (var hexagon in mapMatris)
            {


                hexagon.InstantiatedNeighbourData.FindNeighbours();
            }
        }


       
    }
}
