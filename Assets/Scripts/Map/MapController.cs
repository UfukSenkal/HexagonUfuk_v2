using HexagonDemo.Hexagon;
using HexagonDemo.Match;
using HexagonDemo.Score;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Map
{

    public class MapController : MonoBehaviour
    {
      

        [SerializeField] MapSettings _mapSettings;
        [SerializeField] ScoreController _scoreController;
        [SerializeField] GameObject _gameOverPanel;
        
        private List<int> _instantiatableHexagons = new List<int>();
        private List<GameObject> _instantiatedObjList = new List<GameObject>();

        public GameState currentGameState = MapState.GameStateInfo;
       private IHexagon _bombHexagon;
        private int _bombScore;

        public IHexagon BombHexagon { get { return _bombHexagon; } }

        private void Start()
        {
            _bombScore = _mapSettings.BombScore;
            StartCoroutine(InstantiateInBegining());
        }

        private void Update()
        {
            currentGameState = MapState.GameStateInfo;
            if (MapState.GameStateInfo == GameState.Moving)
            {
                CheckMapIsMoving();
            }

            if (MapState.GameStateInfo == GameState.Explode)
            {
                CheckMapIsEmpty();

            }
            if (_gameOverPanel.activeSelf)
            {
                MapState.GameStateInfo = GameState.GameOver;

            }
            
            
           
        }

        private bool CheckMapIsMoving()
        {

            if (ScriptableSpawnManager.Instance.IsInstantiedAll)
            {
                var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

                foreach (var item in mapMatris)
                {
                    if (item == null || item.InstantiatedHexagonData == null)
                    {

                        StartCoroutine(MoveHexagonsDown());
                        return false;
                    }
                }
                foreach (var item in mapMatris)
                {

                  
                    if (item.InstantiatedHexagonData.IsMoving)
                    {
                        MapState.GameStateInfo = GameState.Moving;
                        return true;
                    }
                }
                MapState.GameStateInfo = GameState.Filled;
                MatchManager.Instance.FindNewNeighbours();
                
            }

            return false;
        }

        public bool CheckMapIsEmpty()
        {
            if (ScriptableSpawnManager.Instance.IsInstantiedAll)
            {
                var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

                foreach (var item in mapMatris)
                {
                    if (item == null)
                    {

                        StartCoroutine(MoveHexagonsDown());
                        return true;
                    }
                }
            }
            
            //CheckMapIsMoving();
            return false;
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
                        
                        
                        yield return new WaitForSeconds(.5f);
                        if (_scoreController.Score >= _bombScore && !CheckBombHexagon())
                        {

                            _bombScore += _mapSettings.BombScore;
                            mapMatris[i, _mapSettings.GridHeight - 1].InstantiatedHexagonData.BombText.text = _mapSettings.BombTime.ToString();
                            mapMatris[i, _mapSettings.GridHeight - 1].InstantiatedHexagonData.BombText.gameObject.SetActive(true);
                            mapMatris[i, _mapSettings.GridHeight - 1].InstantiatedHexagonData.IsBomb = true;
                            _bombHexagon = mapMatris[i, _mapSettings.GridHeight - 1].GetComponent<HexagonController>().InstantiatedHexagonData;


                        }
                        mapMatris[i, _mapSettings.GridHeight - 1].InstantiatedHexagonData.CalculatePosition(i, _mapSettings.GridHeight - 1);

                    }

                    while (mapMatris[i, tempj] == null)
                    {
                        tempj++;
                    }
                    int y = tempj - 1;
                    while (mapMatris[i, y] == null)
                    {

                        try
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
                        catch (Exception)
                        {
                            break;
                        }
                    }


                }
                yield return new WaitForSeconds(.004f);



            }
            if (CheckMapIsEmpty())
            {
                StartCoroutine(MoveHexagonsDown());

                
            }
            while (CheckMapIsMoving())
            {
                yield return null;
            }
            
            if (CheckGameOver())
            {
                _gameOverPanel.SetActive(true);
            }
            else
            {
                MatchManager.Instance.CheckMatchForMap(false);
            }
        }

      public bool CheckBombHexagon()
        {
            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;
            foreach (var item in mapMatris)
            {
                if (item.InstantiatedHexagonData != null)
                {

                    if (item.InstantiatedHexagonData.IsBomb)
                    {
                        _bombHexagon = item.InstantiatedHexagonData;
                        return true;
                    }
                }
            }
            return false;
        }
        #region GameOverLogic

        public void End()
        {
            _gameOverPanel.SetActive(true);
        }

        public bool CheckGameOver()
        {
            var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            foreach (var hexagon in mapMatris)
            {
                hexagon.InstantiatedNeighbourData.FindNeighbours();
                foreach (var _neighbourList in hexagon.InstantiatedNeighbourData.SelectableHexagonList)
                {
                    if (_neighbourList.Count == 3)
                    {

                        if (_neighbourList[0].HexagonColor == _neighbourList[1].HexagonColor
                            || _neighbourList[1].HexagonColor == _neighbourList[2].HexagonColor
                            || _neighbourList[0].HexagonColor == _neighbourList[2].HexagonColor)
                        {

                            bool canMatch = CheckNeighbourForMatch(_neighbourList);
                            if (canMatch)
                            {

                                return false;
                            }
                        }
                    }
                }

            }
            return true;
        }

        public bool CheckNeighbourForMatch(List<IHexagon> _neighbourList)
        {
            if (!_neighbourList.Contains(null))
            {

            
            IHexagon differentObj = _neighbourList[0];
            if (_neighbourList[0].HexagonColor == _neighbourList[1].HexagonColor)
                differentObj = _neighbourList[2];
            else if (_neighbourList[0].HexagonColor == _neighbourList[2].HexagonColor)
                differentObj = _neighbourList[1];
            else if (_neighbourList[1].HexagonColor == _neighbourList[2].HexagonColor)
                differentObj = _neighbourList[0];



                 var denem = _neighbourList.Find(x => x.HexagonColor != differentObj.HexagonColor);
                Color sameColor = Color.white;
                if (denem != null)
                {
                     sameColor = _neighbourList.Find(x => x.HexagonColor != differentObj.HexagonColor).HexagonColor;

                }
               


            List<IHexagon> sameObjects = new List<IHexagon>(_neighbourList);
            sameObjects.Remove(differentObj);



            List<List<IHexagon>> neighbourLists = new List<List<IHexagon>>(differentObj.SelfGameObject.GetComponent<HexagonController>().InstantiatedNeighbourData.SelectableHexagonList);
            foreach (var neighbourList in neighbourLists)
            {


                if (neighbourList.Count == 3 && !ListContains(neighbourList, sameObjects))
                {

                    if (neighbourList[0].HexagonColor == sameColor
                        || neighbourList[1].HexagonColor == sameColor
                        || neighbourList[2].HexagonColor == sameColor)
                    {
                        return true;
                    }
                }
            }
            }
            return false;


        }

        private bool ListContains(List<IHexagon> _list, List<IHexagon> _gameObjList)
        {
            foreach (var item in _list)
            {
                if (_gameObjList.Contains(item))
                    return true;

            }
            return false;
        }

        #endregion
    }


}
