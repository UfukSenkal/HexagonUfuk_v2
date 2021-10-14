using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Map.MapSettings _mapSettings;

        [SerializeField] private float aspectRatioDesign = (16f / 9f);
        [SerializeField] private float closenessMult = .32f;


        void Start()
        {

            aspectRatioDesign = (float)Screen.height / (float)Screen.width;


            Camera.main.orthographicSize = aspectRatioDesign * (_mapSettings.GridWidth) * closenessMult;
            Camera.main.transform.position = new Vector3((Camera.main.orthographicSize * Camera.main.aspect) - (closenessMult * 2f), _mapSettings.GridHeight / 3f, transform.position.z);
        }


    }
}
