using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Map
{

    [CreateAssetMenu(menuName = "HexagonDemo/Map/Map Settings")]
    public class MapSettings : ScriptableObject
    {
        [Header("Map")]
        [SerializeField] private int _gridWidth = 8;
        [SerializeField] private int _gridHeight = 9;
        private int _gridWidthMax = 10;
        private int _gridWidthMin = 6;
        private int _gridHeightMax = 11;
        private int _gridHeightMin = 7;
        private float _gridYOffset = .626f;
        private float _gridXOffset = .562f;
        private float _speed = 2f;


        [Header("Bomb")]
        [SerializeField] private int _bombTime = 7;
        private int _bombTimeMax = 10;
        private int _bombTimeMin = 5;
        private int _bombScore = 1000;

        [Header("Color")]
        [SerializeField] private int _colorCount = 5;
        private int _colorCountMax = 7;
        private int _colorCountMin = 5;
        [SerializeField] private Color[] _colors;


        public int GridWidth { get => _gridWidth; set => _gridWidth = value; }
        public int GridHeight { get => _gridHeight; set => _gridHeight = value; }
        public int ColorCount { get => _colorCount; set => _colorCount = value; }
        public Color[] Colors { get => _colors; set => _colors = value; }
        public int BombTime { get => _bombTime; set => _bombTime = value; }
        public int ColorCountMax { get => _colorCountMax; }
        public int ColorCountMin { get => _colorCountMin; }
        public int GridWidthMax { get => _gridWidthMax;  }
        public int GridWidthMin { get => _gridWidthMin; }
        public int GridHeightMax { get => _gridHeightMax;  }
        public int GridHeightMin { get => _gridHeightMin;  }
        public int BombTimeMax { get => _bombTimeMax;  }
        public int BombTimeMin { get => _bombTimeMin;  }
        public float GridYOffset { get => _gridYOffset; }
        public float GridXOffset { get => _gridXOffset; }
        public int BombScore { get => _bombScore; }
        public float Speed { get => _speed; }
    }
}
