using HexagonDemo.Hexagon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonDemo.InputData
{
    [CreateAssetMenu(menuName = "HexagonDemo/Input/InputData")]
    public class InputData : ScriptableObject
    {
        private List<IHexagon> _lastSelectionList = new List<IHexagon>();
        [SerializeField] private GameObject centerObjPrefab;
        private GameObject _instantiatedCenterObj;

        public void Click()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit && hit.transform.TryGetComponent<HexagonController>(out HexagonController neighbour))
            {
                ClearLastSelection();
                SelectNeighbours(mousePos, neighbour);
                
                
            }

        }

        private void SelectNeighbours(Vector2 mousePos, HexagonController neighbour)
        {
            neighbour.InstantiatedNeighbourData.GetClosestNeighbours(mousePos);
            for (int i = 0; i < neighbour.InstantiatedNeighbourData.GetClosestNeighbours(mousePos).Count; i++)
            {
                neighbour.InstantiatedNeighbourData.GetClosestNeighbours(mousePos)[i].Outline.SetActive(true);
                _lastSelectionList.Add(neighbour.InstantiatedNeighbourData.GetClosestNeighbours(mousePos)[i]);
                

            }


            for (int i = 0; i < _lastSelectionList.Count; i++)
            {
                Debug.Log(i + " : " + _lastSelectionList[i].SelfGameObject.name);
            }

           
         
            
            FindCenter();
            
        }

        public void ClearLastSelection()
        {

           
            for (int i = 0; i < _lastSelectionList.Count; i++)
            {
                if (_lastSelectionList[i].Outline != null)
                {

                    _lastSelectionList[i].Outline.SetActive(false);
                    _lastSelectionList[i].SelfGameObject.transform.SetParent(null);
                }
            }
            Destroy(_instantiatedCenterObj);
            _lastSelectionList.Clear();
        }
        public void RotateHexagons()
        {
            //_instantiatedCenterObj.transform.Rotate(0, 0, 120f, Space.Self);
            //Debug.Log(_instantiatedCenterObj.transform.eulerAngles);
            _instantiatedCenterObj.GetComponent<HexagonRotationController>().RotateHexagons(_lastSelectionList);
            
        }

        private Vector2 FindCenter()
        {
            Vector3 center = Vector2.zero;

            foreach (var neighbour in _lastSelectionList)
            {
                center += neighbour.SelfGameObject.transform.position;
            }
            center /= 3;
            InstantiateCenterObj(center);
            return center;
        }

        private void InstantiateCenterObj(Vector2 center)
        {
           _instantiatedCenterObj = Instantiate(centerObjPrefab);
            _instantiatedCenterObj.transform.position = center;
            foreach (var neighbour in _lastSelectionList)
            {
                neighbour.SelfGameObject.transform.SetParent(_instantiatedCenterObj.transform);
            }

        }

       
    }
}

