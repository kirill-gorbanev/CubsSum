using System.Collections.Generic;
using Code.Grid;
using UnityEngine;

namespace Code.UI.Tools
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private Tools tooltipPrefab;
        [SerializeField] private RectTransform parent;

        [SerializeField] private Spawner spawner;

        public Dictionary<Vector2Int, Tools> tooltips = new();

        private void Awake()
        {
            spawner.OnNewSpawn += Spawn;
        }

        private void Spawn(Spawner.ItemInfo info)
        {
            if (!info.isMain)
                return;

            var p = Instantiate(tooltipPrefab, parent);
            
            tooltips.Add(info.id, p);
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, info.pos);

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parent, screenPoint, Camera.main, out Vector2 localPoint))
                return;


            var rectTransform = p.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = localPoint;
            
            p.gameObject.SetActive(false);
        }
    }
}