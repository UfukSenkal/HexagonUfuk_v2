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
        [SerializeField] ParticleSystem _explosionEffect;
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


                MapState.GameStateInfo = GameState.Explode;
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

        private IEnumerator DestroyNeighbourList(List<IHexagon> _neighbourList)
        {
            ExplosionEffect(_neighbourList);
            _inputData.ClearLastSelection();
            yield return new WaitForSeconds(.2f);
            foreach (var item in _neighbourList)
            {

                Destroy(item.SelfGameObject);
            }
        }

        private void ExplosionEffect(List<IHexagon> _neighbourList)
        {
            _explosionEffect.Stop();

            var pos = FindCenter(_neighbourList);


            _explosionEffect.transform.position = pos;
            var partycleMain = _explosionEffect.main;
            partycleMain.startColor = _neighbourList[0].HexagonColor;

            _explosionEffect.Play();
        }

        void FindNewNeighbours()
        {
            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            foreach (var hexagon in mapMatris)
            {


                hexagon.InstantiatedNeighbourData.FindNeighbours();
            }
        }

        Vector3 FindCenter(List<IHexagon> list)
        {
            Vector3 center = Vector2.zero;

            foreach (var neighbour in list)
            {
                center += neighbour.SelfGameObject.transform.position;
            }
            center /= 3;
            
            return center;
        }


       
    }
}
