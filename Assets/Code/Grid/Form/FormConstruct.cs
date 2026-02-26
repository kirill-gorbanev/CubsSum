using System;
using System.Collections.Generic;
using Code.UI;
using UnityEngine;

namespace Code.Grid.Form
{
    public class FormConstruct : MonoBehaviour
    {
        [SerializeField] public Grid[] grid;
        [SerializeField] public Vector3 offset;
        [SerializeField] public SpriteRenderer previewPr;

        private Vector3 _last;
        private HashSet<Vector3> _pointersHas = new();

        [Serializable]
        public class Grid
        {
            [SerializeField] public Transform cell;
            [SerializeField] public SpriteRenderer sr;
            [HideInInspector] public List<SpriteRenderer> detectSR = new();
        }

        public bool IsGroup { get; set; }
        public Vector3 size { get; set; }
        public Spawner spawner { get; set; }

        public TypeCell[] cells { get; private set; }

        public int Count => grid.Length;

        private void Start()
        {
            _last = offset;
        }

        public void LoadView(TypeCell[] views)
        {
            cells = views;
            int i = 0;
            foreach (var tt in cells)
            {
                grid[i].cell.GetComponent<SpriteRenderer>().color = tt.view;
                grid[i].sr.sprite = tt.sprite;
                grid[i].sr.transform.localScale /= 2;
                i++;
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

            foreach (var g in grid)
                g.sr.transform.rotation = Quaternion.identity;
        }

        public void Move()
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawner.Detect(this);
        }

        public void Drag()
        {
            transform.parent = null;
            transform.localScale = spawner.size;
            transform.Rotate(0, 0, -90);

            if (_pointersHas.Count > 0)
            {
                foreach (var cell in grid)
                foreach (var ss in cell.detectSR)
                    ss.gameObject.SetActive(true);

                return;
            }

            foreach (var g in grid)
                _pointersHas.Add(g.cell.position);

            for (int i = 0; i < cells.Length; i++)
            {
                var cell = cells[i];
                foreach (var point in cell.pointers.pointsDetect)
                {
                    var p = grid[i].cell.position + new Vector3(point.position.x * spawner.size.x,
                        point.position.y * spawner.size.y);
                    if (!_pointersHas.Add(p))
                        continue;
                    var ss = Instantiate(previewPr, p, Quaternion.identity, grid[i].cell.transform);

                    grid[i].detectSR.Add(ss);

                    ss.color = Color.black;
                    ss.gameObject.SetActive(true);
                }
            }

            foreach (var g in grid)
                g.sr.transform.rotation = Quaternion.identity;
        }

        public bool FindCellReset(Prefab prefab, GameObject eventDataPointerEnter)
        {
            foreach (var g in grid)
                g.sr.transform.rotation = Quaternion.identity;

            foreach (var cell in grid)
            foreach (var ss in cell.detectSR)
                ss.gameObject.SetActive(false);

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

        public void Preview(int idCell, int id, bool active)
        {
            grid[idCell].detectSR[id].color = active ? Color.green : Color.black;
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
}