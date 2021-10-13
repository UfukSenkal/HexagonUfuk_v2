using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo.Hexagon
{
    public abstract class AbstractBaseHexagonData : ScriptableObject
    {
        protected HexagonController _hexagonController;
        public virtual void Initialize(HexagonController hexagonController)
        {
            _hexagonController = hexagonController;
        }

        public virtual void Destroy()
        {
            Destroy(this);
        }
    }
}
