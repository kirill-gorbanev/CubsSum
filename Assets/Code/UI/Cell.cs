using Code.InputSearch;
using Code.Signal;
using Code.UI.Scroll;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Cell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IFilt
    {
        [SerializeField] private Image image;

        private CreateCells _createCells;

        public Color ImageColor { get; private set; }
        public GameObject Item => gameObject;

        public void Init(Color c, CreateCells createCells) =>
            (image.color, _createCells) = (ImageColor = c, createCells);

        public void OnBeginDrag(PointerEventData eventData)
        {
            _createCells.ServerDown.Execute(new Down { transform = transform, point = eventData.position }, this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _createCells.ServerDrag.Execute(new Drag { item = transform, origin = eventData.position }, this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _createCells.ServerUp.Execute(new Up { transform = transform, point = eventData.position }, this);
        }
    }
}