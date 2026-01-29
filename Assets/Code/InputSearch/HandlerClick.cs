using Code.Signal;
using UnityEngine;

namespace Code.InputSearch
{
    public class HandlerClick : MonoBehaviour
    {
        private bool _isDrag;

        private Camera _camera;
        private IFilt _item;

        public Server<Drag> ServerDrag { get; } = new();
        public Server<Up> ServerUp { get; } = new();

        private void Start()
        {
            _camera = Camera.main;
        }

        private Vector2 Origin => _camera.ScreenToWorldPoint(Input.mousePosition);

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Origin, Vector2.zero);

                if (hit.collider != null && hit.collider.TryGetComponent(out IFilt filt))
                {
                    _item = filt;
                    _isDrag = true;
                }
            }
            else if (Input.GetMouseButton(0) && _isDrag)
            {
                ServerDrag.Execute(new Drag { item = _item.Item.transform, origin = Origin }, _item);
            }
            else if (Input.GetMouseButtonUp(0) && _isDrag)
            {
                ServerUp.Execute(new Up { transform = _item.Item.transform }, _item);

                _isDrag = false;
                _item = null;
            }
        }
    }
}