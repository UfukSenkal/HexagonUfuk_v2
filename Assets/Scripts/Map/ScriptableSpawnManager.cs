﻿using HexagonDemo.Hexagon;
using HexagonDemo.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo
{
    [CreateAssetMenu(menuName = "HexagonDemo/Manager/Scriptable Spawn Manager")]
    public class ScriptableSpawnManager : AbstractScriptableManager<ScriptableSpawnManager>
    {
        [SerializeField] private GameObject _hexagonPrefab;
        [SerializeField] private Vector2 _startPos;
        [SerializeField] private MapSettings _mapSettings;
        
        private HexagonController[,] _mapMatris;
        public HexagonController[,] MapMatris { get { return _mapMatris; } }

     

     
        public override void Initialize()
        {
            base.Initialize();
            _mapMatris = new HexagonController[_mapSettings.GridWidth, _mapSettings.GridHeight];
        }

        public GameObject InstantiateHexagon(int x, int y)
        {
           
            GameObject hexagon = Instantiate(_hexagonPrefab, _startPos, Quaternion.identity);
            hexagon.name = "Hexagon " + x + " - " + y;
            var hexagonController = hexagon.GetComponent<HexagonDemo.Hexagon.HexagonController>();
            hexagonController.x = x;
            hexagonController.y = y;
            _mapMatris[x, y] = hexagonController;


            return hexagon;
        }

        
    }
}