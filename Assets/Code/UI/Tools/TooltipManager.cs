using Code.Grid;
using UnityEngine;

namespace Code.UI.Tools
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private GameObject tooltipPrefab;
        [SerializeField] private RectTransform parent;

        [SerializeField] private Spawner spawner;
        [SerializeField] private Vector2 offset;

        private void Awake()
        {
            spawner.OnNewSpawn += Spawn;
        }

        private void Spawn(Vector2Int pos)
        {
            var p = Instantiate(tooltipPrefab, parent);
            
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, pos+ offset);

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parent, screenPoint, Camera.main, out Vector2 localPoint))
                return;


            var rectTransform = p.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = localPoint ;
        }
    }
}