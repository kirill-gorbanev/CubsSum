using Code.UI;
using UnityEngine;

namespace Code.Grid.Form
{
    public class FormConstruct : MonoBehaviour
    {
        [SerializeField] private Transform[] grid;
        [SerializeField] public Vector3 offset;
        [HideInInspector] public Vector3 size;

        private Vector2[] _gridPos;

        private void Start()
        {
            _last = offset;
            Select();
        }

        private void OnValidate()
        {
            Select();
        }

        private void Select()
        {
            var s = grid.Length;
            _gridPos = new Vector2[s];
            for (int i = 0; i < s; i++)
            {
                var g = grid[i];
                _gridPos[i] = g.position - transform.position;
            }
        }

        private void OnDrawGizmos()
        {
            if (_gridPos == null)
                return;
            Gizmos.color = Color.yellow;
            foreach (var p in _gridPos)
            {
                Gizmos.DrawCube(p, Vector3.one / 3);
            }
        }

        private Vector3 _last;

        public void Rotate()
        {
            transform.position -= _last;
            transform.Rotate(0, 0, 90);

            Quaternion rotation = transform.rotation;
            Vector3 rotatedDirection = rotation * offset;

            transform.position += rotatedDirection;
            _last = rotatedDirection;

            Select();
        }

        public void Move()
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void Drag()
        {
            transform.parent = null;
            transform.localScale = Vector3.one;
            transform.Rotate(0, 0, -90);
        }

        public bool FindCellReset(Prefab prefab)
        {
            transform.parent = prefab.transform;
            transform.localScale = size;

            Quaternion rotation = transform.rotation;
            Vector3 rotatedDirection = rotation * offset;

            transform.position = prefab.transform.position + rotatedDirection;
            _last = rotatedDirection;

            return false;
        }
    }
}