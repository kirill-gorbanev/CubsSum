using System;
using Code.Build;
using Code.UI;
using UnityEngine;

namespace Code.Sector.UI
{
    [Serializable]
    public struct IsTower : ISect
    {
        [SerializeField] private CellInteract cellInteract;

        public bool Execute()
        {
            if (!cellInteract.Hit.TryGetComponent(out Tower connectHandler))
                connectHandler = cellInteract.Hit.GetComponentInParent<Tower>();

            if (connectHandler == null)
                return false;

            cellInteract.Tower = connectHandler;
            return true;
        }
    }
}