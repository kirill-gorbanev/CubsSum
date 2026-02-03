using Code.Grid.Form;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI
{
    public class Prefab : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public FormConstruct Content;

        public void OnPointerDown(PointerEventData eventData)
        {
            Content.Rotate();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Content.Drag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Content.Move();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var b = Content.FindCellReset(this);
            if (b)
                Destroy(gameObject);
        }
    }
}