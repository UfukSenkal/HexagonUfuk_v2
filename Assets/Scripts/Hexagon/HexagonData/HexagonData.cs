using HexagonDemo.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Hexagon
{
    [CreateAssetMenu(menuName = "HexagonDemo/Hexagon/HexagonData")]
    public class HexagonData : AbstractBaseHexagonData, IHexagon
    {
        [SerializeField] private MapSettings _mapSettings;
        [SerializeField] private float _speed;

        public float Speed { get { return _speed; } }
        public MapSettings MapSettings { get { return _mapSettings; } }
       

        public int X { get; set; }

        public int Y { get; set; }
        public Color HexagonColor { get; set; }

        public Vector2 HexagonPosition { get; set; }

        public Vector2 HexagonStartPosition { get; set; }

        public SpriteRenderer HexagonSpriteRenderer { get; set; }

        public GameObject Outline { get; set; }

        public GameObject SelfGameObject { get; set; }
        public bool IsMoving { get; set; }

        public override void Initialize(HexagonController hexagonController)
        {
            
            base.Initialize(hexagonController);
            SelfGameObject = hexagonController.gameObject;
            X = hexagonController.x;
            Y = hexagonController.y;
            CalculatePosition(X, Y);
            HexagonColor = CheckStartColor(X,Y);
            ChangeColor(hexagonController,this);
            HexagonStartPosition = new Vector2(HexagonPosition.x, 10f);
            
            

        }
        public void ChangeColor(HexagonController hexController,IHexagon hex)
        {
            HexagonColor = hex.HexagonColor;
            hexController.SpriteRenderer.color = HexagonColor;
            
        }
    

       
        public void CalculatePosition(int x, int y)
        {
            float yPos = y * _mapSettings.GridYOffset;
            float xPos = x * _mapSettings.GridXOffset;
            if (x % 2 == 0)
            {
                yPos += _mapSettings.GridYOffset / 2;

            }

            HexagonPosition = new Vector2(xPos, yPos);

        }
        public Color CheckStartColor(int i, int j)
        {
            var _mapMatris = ScriptableSpawnManager.Instance.MapMatris;

            Color goColor = _mapSettings.Colors[Random.Range(0, _mapSettings.Colors.Length)];
            if (Time.fixedTime < 3f)
            {
                if (i > 0)
                {

                    if (i % 2 == 0)
                    {
                        while (_mapMatris[i - 1, j].GetComponent<Hexagon.HexagonController>().SpriteRenderer.color == goColor)
                        {
                            goColor = _mapSettings.Colors[Random.Range(0, _mapSettings.Colors.Length)];
                        }
                    }
                    else
                    {
                        if (j > 0)
                        {

                            while (_mapMatris[i - 1, j - 1].GetComponent<Hexagon.HexagonController>().SpriteRenderer.color == goColor)
                            {
                                goColor = _mapSettings.Colors[Random.Range(0, _mapSettings.Colors.Length)];
                            }
                        }
                    }
                }
            }
            return goColor;


            
        }

    }
}
