using HexagonDemo.Match;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonDemo.Hexagon {

    [CreateAssetMenu(menuName = "HexagonDemo/Hexagon/Neighbour Data")]
    public class NeighbourData : AbstractBaseHexagonData
    {
        private IHexagon _selfHexagon;
        private IHexagon _neighbourHexagonUp;
        private IHexagon _neighbourHexagonUpRight;
        private IHexagon _neighbourHexagonUpLeft;
        private IHexagon _neighbourHexagonDown;
        private IHexagon _neighbourHexagonDownRight;
        private IHexagon _neighbourHexagonDownLeft;

        private List<IHexagon> _matchableList;
        private List<IHexagon> _matchList;
        private List<IHexagon> _neighbourRightList;
        private List<IHexagon> _neighbourUpRightList;
        private List<IHexagon> _neighbourUpLeftList;
        private List<IHexagon> _neighbourDownLeftList;
        private List<IHexagon> _neighbourDownRightList;
        private List<IHexagon> _neighbourLeftList;

        private List<List<IHexagon>> _selectableHexagonList;

        public List<List<IHexagon>> SelectableHexagonList { get { return _selectableHexagonList; } }
        public List<IHexagon> MatchList { get { return _matchList; } }
       

        public override void Initialize(HexagonController hexagonController)
        {
            base.Initialize(hexagonController);
            _selfHexagon = hexagonController.InstantiatedHexagonData;
        }

        public void FindNeighbours()
        {
             _neighbourHexagonUp = null;
             _neighbourHexagonUpRight = null;
             _neighbourHexagonUpLeft = null;
             _neighbourHexagonDown = null;
             _neighbourHexagonDownRight = null;
             _neighbourHexagonDownLeft = null;


        var mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            bool isUpperHeight = (_selfHexagon.X % 2 == 0) ? true : false;

            int x = _selfHexagon.X;
            int y = _selfHexagon.Y;

            try
            {


                if (y != (_hexagonController.InstantiatedHexagonData.MapSettings.GridHeight - 1))
                {
                    _neighbourHexagonUp = mapMatris[x, y + 1].InstantiatedHexagonData;
                }

                if (x != (_hexagonController.InstantiatedHexagonData.MapSettings.GridWidth - 1) && (isUpperHeight ? y != (_hexagonController.InstantiatedHexagonData.MapSettings.GridHeight - 1) : true))
                {


                    _neighbourHexagonUpRight = mapMatris[x + 1, isUpperHeight ? y + 1 : y].InstantiatedHexagonData;

                }

                if (x != 0 && (isUpperHeight ? y != (_hexagonController.InstantiatedHexagonData.MapSettings.GridHeight - 1) : true))
                {

                    _neighbourHexagonUpLeft = mapMatris[x - 1, isUpperHeight ? y + 1 : y].InstantiatedHexagonData;

                }

                if (y != 0)
                {
                    _neighbourHexagonDown = mapMatris[x, y - 1].InstantiatedHexagonData;

                }

                if (x != (_hexagonController.InstantiatedHexagonData.MapSettings.GridWidth - 1) && (isUpperHeight ? true : y != 0))
                {
                    _neighbourHexagonDownRight = mapMatris[x + 1, isUpperHeight ? y : y - 1].InstantiatedHexagonData;

                }

                if (x != 0 && (isUpperHeight ? true : y != 0))
                {
                    _neighbourHexagonDownLeft = mapMatris[x - 1, isUpperHeight ? y : y - 1].InstantiatedHexagonData;

                }


                FindSelectableNeighbours();

                FindMatchNeighbourList();
            }
            catch (System.Exception)
            {

            }
        }


        void FindSelectableNeighbours()
        {
            _neighbourRightList = new List<IHexagon>();
            _neighbourUpRightList = new List<IHexagon>();
            _neighbourUpLeftList = new List<IHexagon>();
            _neighbourDownLeftList = new List<IHexagon>();
            _neighbourDownRightList = new List<IHexagon>();
            _neighbourLeftList = new List<IHexagon>();

            _matchableList = new List<IHexagon>();
            _matchList = new List<IHexagon>();

            _selectableHexagonList = new List<List<IHexagon>>();

            _neighbourRightList.Add(_selfHexagon);
            _neighbourUpRightList.Add(_selfHexagon);
            _neighbourUpLeftList.Add(_selfHexagon);
            _neighbourDownLeftList.Add(_selfHexagon);
            _neighbourDownRightList.Add(_selfHexagon);
            _neighbourLeftList.Add(_selfHexagon);


            if (_neighbourHexagonUp != null)
            {
                _matchableList.Add(_neighbourHexagonUp);
                _neighbourUpRightList.Add(_neighbourHexagonUp);
                _neighbourUpLeftList.Add(_neighbourHexagonUp);
            }
            if (_neighbourHexagonUpLeft != null)
            {
                _matchableList.Add(_neighbourHexagonUpLeft);
                _neighbourUpLeftList.Add(_neighbourHexagonUpLeft);
                _neighbourLeftList.Add(_neighbourHexagonUpLeft);

            }
            if (_neighbourHexagonDownLeft != null)
            {
                _matchableList.Add(_neighbourHexagonDownLeft);
                _neighbourDownLeftList.Add(_neighbourHexagonDownLeft);
                _neighbourLeftList.Add(_neighbourHexagonDownLeft);
            }
            if (_neighbourHexagonDown != null)
            {
                _matchableList.Add(_neighbourHexagonDown);
                _neighbourDownLeftList.Add(_neighbourHexagonDown);
                _neighbourDownRightList.Add(_neighbourHexagonDown);
            }
         
            if (_neighbourHexagonDownRight != null)
            {
                _matchableList.Add(_neighbourHexagonDownRight);
                _neighbourRightList.Add(_neighbourHexagonDownRight);
                _neighbourDownRightList.Add(_neighbourHexagonDownRight);
            }
            if (_neighbourHexagonUpRight != null)
            {
                _matchableList.Add(_neighbourHexagonUpRight);
                _neighbourUpRightList.Add(_neighbourHexagonUpRight);
                _neighbourRightList.Add(_neighbourHexagonUpRight);
            }





            _neighbourUpRightList = _neighbourUpRightList.OrderByDescending(p => p.X).ToList().FindAll(t => t.X == _selfHexagon.X).OrderByDescending(l => l.Y).ToList();
            

            _selectableHexagonList.Add(_neighbourRightList);
            //_selectableHexagonList.Add(_neighbourUpRightList);
            _selectableHexagonList.Add(_neighbourUpLeftList);
           _selectableHexagonList.Add(_neighbourDownLeftList);
            _selectableHexagonList.Add(_neighbourDownRightList);
            _selectableHexagonList.Add(_neighbourLeftList);


            
        }

        public void FindMatchNeighbourList()
        {

            List<IHexagon> matchListTemp = new List<IHexagon>();
            _selectableHexagonList = _selectableHexagonList.OrderByDescending(p => p.Count).ToList();

            var matchList = new List<IHexagon>();

            for (int i = 0; i < _matchableList.Count - 1; i++)
            {
                if (_matchableList[i].HexagonColor == _selfHexagon.HexagonColor)
                {
                    matchList.Add(_matchableList[i]);
                    if (_matchableList[((i + 1) == _matchableList.Count) ? i - 1 : i + 1].HexagonColor == _selfHexagon.HexagonColor)
                    {
                        matchList.Add(_matchableList[((i + 1) == _matchableList.Count) ? i - 1 : i + 1]);
                    }
                    else
                    {
                        matchList.Remove(_matchableList[i]);
                    }
                }
            }

            foreach (var _neighbourList in _selectableHexagonList)
            {
                if (_neighbourList.Count >= 3)
                {

                    if (_neighbourList.FindAll(p => p.HexagonColor == _selfHexagon.HexagonColor).Count == _neighbourList.Count)
                    {
                         matchListTemp = _neighbourList;
                        MapState.GameStateInfo = GameState.Match;
                        

                        break;
                    }
                }
            }


            _matchList = matchListTemp;
  

          
        }
        public List<IHexagon> GetClosestNeighbours(Vector2 mousePos)
        {
            float distance = 10f;
            Vector3 tempPos = Vector3.zero;

            List<IHexagon> closestNeighbours = new List<IHexagon>();

            foreach (var list in _selectableHexagonList)
            {
                if (list != null && list.Count >= 3)
                {

                    foreach (var neighbour in list)
                    {
                        tempPos += neighbour.SelfGameObject.transform.position;
                    }
                    tempPos /= 3;

                    if (Vector2.Distance(mousePos, tempPos) < distance)
                    {
                        distance = Vector2.Distance(mousePos, tempPos);
                        closestNeighbours = list;
                       
                    }
                    tempPos = Vector2.zero;
                }
            }


            return closestNeighbours;
        }
     
    }
}
