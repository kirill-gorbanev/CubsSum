using System;
using Code.UI;
using UnityEngine;

namespace Code.Grid.Form
{
    public class FormConstruct : MonoBehaviour
    {
        [SerializeField] public Transform[] grid;
        [SerializeField] public Vector3 offset;

        public bool IsGroup { get; set; }
        public Vector3 size { get; set; }
        public Spawner spawner { get; set; }

        public TypeCell[] cells { get; private set; }

        public int Count => grid.Length;

        private void Start()
        {
            _last = offset;
        }

        private Vector3 _last;

        public void LoadView(ViewCell[] views)
        {
            if (views.Length != Count)
                return;

            cells = new TypeCell[Count];
            for (int i = 0; i < Count; i++)
            {
                var v = views[i];
                cells[i].id = v.id;

                Instantiate(v.prefab, grid[i].position, Quaternion.identity, grid[i]);
            }
        }

        public void Rotate()
        {
            transform.position -= _last;
            transform.Rotate(0, 0, 90);

            Quaternion rotation = transform.rotation;
            Vector3 rotatedDirection = rotation * offset;

            transform.position += rotatedDirection;
            _last = rotatedDirection;
        }

        public void Move()
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void Drag()
        {
            transform.parent = null;
            transform.localScale = spawner.size;
            transform.Rotate(0, 0, -90);
        }

        public bool FindCellReset(Prefab prefab, GameObject eventDataPointerEnter)
        {
            if (eventDataPointerEnter != null && eventDataPointerEnter.TryGetComponent(out CollectItem coll) &&
                coll.Content == null)
            {
                coll.Content = this;
                Reset(coll.transform);
                return true;
            }


            var rez = spawner.Connect(this);
            if (rez == null)
            {
                Reset(prefab.transform);
                return false;
            }

            transform.position = rez[0].pos;

            spawner.SetPos(rez);


            return true;
        }

        private void Reset(Transform tr)
        {
            transform.parent = tr;
            transform.localScale = size;

            Quaternion rotation = transform.rotation;
            Vector3 rotatedDirection = rotation * offset;

            transform.position = tr.position + rotatedDirection;
            _last = rotatedDirection;
        }
    }

    [Serializable]
    public struct TypeCell
    {
        public int id;
    }

    [Serializable]
    public struct ViewCell
    {
        public int id;
        public GameObject prefab;
    }
}