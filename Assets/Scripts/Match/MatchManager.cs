using HexagonDemo.Hexagon;
using HexagonDemo.Map;
using HexagonDemo.Score;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonDemo.Match
{

    public class MatchManager : MonoBehaviour
    {
        public static MatchManager Instance;
        private bool _onMove = false;

        [SerializeField] InputData.InputData _inputData;
        [SerializeField] ParticleSystem _explosionEffect;
        [SerializeField] ScoreController _scoreController;
        [SerializeField] MapController _mapController;
      
        private void Awake()
        {
            Instance = this;
        }


        public void CheckMatchForMap(bool onMove)
        {
            _onMove = onMove;
            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            foreach (var hexagon in mapMatris)
            {
                if (hexagon != null && hexagon.InstantiatedNeighbourData != null)
                {

                    hexagon.InstantiatedNeighbourData.FindNeighbours();
                    CheckMatch(hexagon);
                    if (MapState.GameStateInfo == GameState.Explode)
                    {
                        break;
                    }
                }


            }
        }

        private void CheckMatch(HexagonController hexagon)
        {
            
                if (hexagon.InstantiatedNeighbourData.MatchList != null && hexagon.InstantiatedNeighbourData.MatchList.Count >= 3)
                {


                MapState.GameStateInfo = GameState.Explode;
                StartCoroutine(DestroyNeighbourList(hexagon.InstantiatedNeighbourData.MatchList));

                }
            
        }

        private IEnumerator DestroyNeighbourList(List<IHexagon> _neighbourList)
        {
            
            if ( !_mapController.CheckMapIsEmpty() && _onMove)
            {
               
                
                _inputData.ClearLastSelection();
                
                if (_mapController.BombHexagon != null)
                {
                    _mapController.BombHexagon.BombTime--;
                    _mapController.BombHexagon.BombText.text = _mapController.BombHexagon.BombTime.ToString();
                    if (_mapController.BombHexagon.BombTime <= 0 && _mapController.BombHexagon.SelfGameObject != null)
                    {
                        _mapController.End();
                    }
                }
            }
            foreach (var item in _neighbourList)
            {
                Destroy(item.SelfGameObject.GetComponent<HexagonController>().InstantiatedNeighbourData);
                Destroy(item.SelfGameObject);
            }
            _scoreController.ScoreTextUpdate(_neighbourList.Count);
            ExplosionEffect(_neighbourList);
            _mapController.CheckMapIsEmpty();
            yield return new WaitForSeconds(.5f);
           
            
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

        public void FindNewNeighbours()
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
