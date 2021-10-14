using HexagonDemo.Hexagon;
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
                CheckMatchForMap();
            }
        }

        private void CheckMatchForMap()
        {
            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            foreach (var hexagon in mapMatris)
            {


                hexagon.InstantiatedNeighbourData.FindNeighbours();
                CheckMatch(hexagon);
                if (MapState.GameStateInfo == GameState.Explode)
                {
                    break;
                }

            }
        }

        private void CheckMatch(HexagonController hexagon)
        {
            
                if (hexagon.InstantiatedNeighbourData.MatchList.Count >= 3)
                {

                    
                       
                        StartCoroutine(DestroyNeighbourList(hexagon.InstantiatedNeighbourData.MatchList));
                        //if (bombHexagon != null)
                        //{
                        //    bombTime--;
                        //    bombHexagon.GetComponent<Hexagon.HexagonController>().SetBombText(bombTime.ToString());
                        //}
                        //int score = (_neighbourList.Count * scoreController.ScoreMult);

                        //scoreController.Score += score;
                        //scoreController.ScoreTextUpdate();
                       
                    
                }
            
        }

        private IEnumerator DestroyNeighbourList(List<Hexagon.IHexagon> _neighbourList)
        {
                _inputData.ClearLastSelection();
            yield return new WaitForSeconds(.3f);
            MapState.GameStateInfo = GameState.Explode;
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
