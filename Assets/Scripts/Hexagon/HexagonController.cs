using HexagonDemo.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Hexagon
{
    public class HexagonController : MonoBehaviour
    {

        //Test
        public bool test;
        public bool testMovement = true;
        public int x, y;
     

        [SerializeField] MapSettings _mapSettings;
        [SerializeField] HexagonData _hexagonData;
        [SerializeField] NeighbourData _neighbourData;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] HexagonMovementController _hexagonMovementController;
        [SerializeField] HexagonRotationController _hexagonRotationController;
        [SerializeField] private GameObject _outline;


        private HexagonData _instantiatedHexagonData;
        private NeighbourData _instantiatedneighbourData;
        

        public HexagonData InstantiatedHexagonData { get { return _instantiatedHexagonData; } }
        public NeighbourData InstantiatedNeighbourData { get { return _instantiatedneighbourData; } }

        public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

        private void Start()
        {
            _instantiatedHexagonData = Instantiate(_hexagonData);
            _instantiatedHexagonData.Initialize(this);
            _instantiatedHexagonData.Outline = _outline;
            _instantiatedHexagonData.SelfGameObject = this.gameObject;

             _instantiatedneighbourData =  Instantiate(_neighbourData);
            _instantiatedneighbourData.Initialize(this);
            transform.position = _instantiatedHexagonData.HexagonStartPosition;
        }

        private void Update()
        {
            //gamestate bağlı moving
            if (MapState.GameStateInfo == GameState.Moving)
            {
                if (Vector3.Distance(transform.position, _instantiatedHexagonData.HexagonPosition) >= .1f)
                {
                    _hexagonMovementController.Move(_instantiatedHexagonData.HexagonPosition, _instantiatedHexagonData.Speed);
                    _instantiatedHexagonData.IsMoving = true;
                }
                else
                {
                    _instantiatedHexagonData.IsMoving = false;
                }
            }

            if (MapState.GameStateInfo == GameState.Filled || MapState.GameStateInfo == GameState.Rotating)
            {
                _instantiatedneighbourData.FindNeighbours();
                
               
                
            }
            
        }

       



        
    }
}
