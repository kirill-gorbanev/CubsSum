using System;
using Code.UI;
using UnityEngine;

namespace Code.Sector.UI
{
    [Serializable]
    public struct IsHit : ISect
    {
        [SerializeField] private Camera camera;
        [SerializeField] private CellInteract cellInteract;

        public bool Execute()
        {
            Vector2 mouseScreenPos = Input.mousePosition;
            Vector2 mouseWorldPos = camera.ScreenToWorldPoint(mouseScreenPos);
            Vector2 origin = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);

            if (hit.collider != null)
            {
                cellInteract.Hit = hit.collider.gameObject;
                return true;
            }

            return false;
        }
    }
}