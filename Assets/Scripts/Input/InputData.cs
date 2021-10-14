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
        private Vector2 firstPos;
        private Vector2 lastPos;
        private Vector2 swipe;
        

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


           
         
            
            FindCenter();
            
        }

        public void Swipe()
        {


            Touch touch = Input.GetTouch(0);

         

                if (touch.phase == TouchPhase.Began)
                {

                    firstPos = touch.position;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    lastPos = touch.position;

                    swipe = new Vector2(lastPos.x - firstPos.x, lastPos.y - firstPos.y);

                    if (Mathf.Abs(lastPos.x - firstPos.x) > Screen.height * 5 / 100 || Mathf.Abs(lastPos.y - firstPos.y) > Screen.height * 5 / 100)
                    {

                        RotateHexagons();

                    }
                    else
                    {
                        Vector2 mousePos = Camera.main.ScreenToWorldPoint(firstPos);
                        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                        if (hit && hit.transform.TryGetComponent<HexagonController>(out HexagonController neighbour))
                        {
                            ClearLastSelection();
                            SelectNeighbours(mousePos, neighbour);


                        }
                    }
                }
            



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

