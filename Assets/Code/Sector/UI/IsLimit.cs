using System;
using Code.InputSearch;
using Code.UI;
using UnityEngine;

namespace Code.Sector.UI
{
    [Serializable]
    public struct IsLimit : ISect
    {
        [SerializeField] private CellInteract cellInteract;
        [SerializeField] private Camera camera;

        public bool Execute()
        {
            float cameraTop = camera.transform.position.y + camera.orthographicSize;
            if (cellInteract.Tower.Up.y + cellInteract.Tower.Size.y > cameraTop)
            {
                cellInteract.ServerOver.Execute(new Over { isLimit = true }, cellInteract);
                return false;
            }

            return true;
        }
    }
}