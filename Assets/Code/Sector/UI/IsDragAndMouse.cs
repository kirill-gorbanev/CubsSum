using System;
using Code.UI;
using UnityEngine;

namespace Code.Sector.UI
{
    [Serializable]
    public struct IsDragAndMouse : ISect
    {
        [SerializeField] private CellInteract cellInteract;

        public bool Execute() => cellInteract.IsDrag && Input.GetMouseButtonUp(0);
    }
}