using HexagonDemo.Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexagonDemo.Hexagon
{
    public class HexagonController : MonoBehaviour
    {


     

        [SerializeField] MapSettings _mapSettings;
        [SerializeField] HexagonData _hexagonData;
        [SerializeField] NeighbourData _neighbourData;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshProUGUI _bombText;
        [SerializeField] HexagonMovementController _hexagonMovementController;
        [SerializeField] private GameObject _outline;
        [SerializeField] private int _x;
        [SerializeField] private int _y;

        private HexagonData _instantiatedHexagonData;
        private NeighbourData _instantiatedneighbourData;
        
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }
        public HexagonData InstantiatedHexagonData { get { return _instantiatedHexagonData; } }
        public NeighbourData InstantiatedNeighbourData { get { return _instantiatedneighbourData; } }

        public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

        private void Start()
        {
            _instantiatedHexagonData = Instantiate(_hexagonData);
            _instantiatedHexagonData.Initialize(this);
            _instantiatedHexagonData.Outline = _outline;
            _instantiatedHexagonData.SelfGameObject = this.gameObject;
            _instantiatedHexagonData.BombText = _bombText;
            _instantiatedHexagonData.BombTime = _mapSettings.BombTime;

             _instantiatedneighbourData =  Instantiate(_neighbourData);
            _instantiatedneighbourData.Initialize(this);
            transform.position = _instantiatedHexagonData.HexagonStartPosition;
        }

        private void Update()
        {
            //gamestate bağlı moving
            if (MapState.GameStateInfo == GameState.Moving)
            {
                InstantiatedHexagonData.CalculatePosition(X,Y);
               _hexagonMovementController.Move(_instantiatedHexagonData.HexagonPosition, _instantiatedHexagonData.Speed);
                if (Vector3.Distance(transform.position, _instantiatedHexagonData.HexagonPosition) >= .01f)
                {
                    _instantiatedHexagonData.IsMoving = true;
                }
                else
                {
                    _instantiatedHexagonData.IsMoving = false;
                }
            }

            
        }

       



        
    }
}
