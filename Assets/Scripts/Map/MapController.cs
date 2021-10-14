using HexagonDemo.Hexagon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Map
{

    public class MapController : MonoBehaviour
    {



        [SerializeField] MapSettings _mapSettings;
        
        private List<int> _instantiatableHexagons = new List<int>();
        private List<GameObject> _instantiatedObjList = new List<GameObject>();

        public GameState currentGameState = MapState.GameStateInfo;

        private void Start()
        {
            StartCoroutine(InstantiateInBegining());
        }

        private void Update()
        {
            if (MapState.GameStateInfo == GameState.Moving)
            {
                CheckMapIsMoving();
            }

            if (MapState.GameStateInfo == GameState.Explode)
            {
                CheckMapIsEmpty();
                //StartCoroutine(MoveHexagonsDown());
            }
            currentGameState = MapState.GameStateInfo;

        }

        private void CheckMapIsMoving()
        {

            if (ScriptableSpawnManager.Instance.IsInstantiedAll)
            {
                var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

                foreach (var item in mapMatris)
                {
                    if (item == null)
                    {

                        StartCoroutine(MoveHexagonsDown());
                        return;
                    }
                }
                foreach (var item in mapMatris)
                {

                  
                    if (item.InstantiatedHexagonData.IsMoving)
                    {
                        MapState.GameStateInfo = GameState.Moving;
                        return;
                    }
                }
                MapState.GameStateInfo = GameState.Filled;
            }

            
        }

        void CheckMapIsEmpty()
        {
            if (ScriptableSpawnManager.Instance.IsInstantiedAll)
            {
                var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

                foreach (var item in mapMatris)
                {
                    if (item == null)
                    {

                        StartCoroutine(MoveHexagonsDown());
                        return;
                    }
                }
            }
            CheckMapIsMoving();
        }

        private IEnumerator InstantiateInBegining()
        {
            MapState.GameStateInfo = GameState.Moving;
            
            for (int i = 0; i < _mapSettings.GridWidth; i++)
            {
                _instantiatableHexagons = new List<int>();

                for (int j = 0; j < _mapSettings.GridHeight; j++)
                {
                    
                    
                    _instantiatableHexagons.Add(j);
                }
                StartCoroutine(InstantiateHexagons(_instantiatableHexagons,i));
                yield return new WaitForSeconds(.45f);
            }
            
        }

        private IEnumerator InstantiateHexagons(List<int> _instantiateHexList,int x)
        {


            for (int i = 0; i < _instantiateHexList.Count; i++)
            {
                var instantiated = ScriptableSpawnManager.Instance.InstantiateHexagon(x, i);
                _instantiatedObjList.Add(instantiated);
                yield return new WaitForSeconds(.25f);
            }
        }

        public IEnumerator MoveHexagonsDown()
        {
            MapState.GameStateInfo = GameState.Moving;

            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;
            for (int i = 0; i < _mapSettings.GridWidth; i++)
            {
                for (int j = _mapSettings.GridHeight - 1; j > 0; j--)
                {

                    int tempj = j;
                    if (mapMatris[i, _mapSettings.GridHeight - 1] == null)
                    {

                        tempj = _mapSettings.GridHeight - 1;
                        ScriptableSpawnManager.Instance.InstantiateHexagon(i, _mapSettings.GridHeight - 1);
                        yield return new WaitForSeconds(.2f);
                        mapMatris[i, _mapSettings.GridHeight - 1].InstantiatedHexagonData.CalculatePosition(i, _mapSettings.GridHeight - 1);

                    }

                    while (mapMatris[i, tempj] == null)
                    {
                        tempj++;
                    }
                    int y = tempj - 1;
                    while (mapMatris[i, y] == null)
                    {


                        mapMatris[i, tempj].InstantiatedHexagonData.CalculatePosition(i, y);


                        mapMatris[i, y] = mapMatris[i, tempj];
                        mapMatris[i, y].name = "Hexagon" + " "  + i + " - " + y;
                        mapMatris[i, y].InstantiatedHexagonData.X = i;
                        mapMatris[i, y].InstantiatedHexagonData.Y = y; 
                        mapMatris[i, y].X = i;
                        mapMatris[i, y].Y = y;
                        mapMatris[i, tempj] = null;
                        tempj = y;
                        y--;
                        if (y < 0)
                        {

                            break;
                        }
                    }


                }
                yield return new WaitForSeconds(.004f);



            }
            CheckMapIsEmpty();
        }

      
    }
}
