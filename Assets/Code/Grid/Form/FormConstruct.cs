using System;
using Code.UI;
using UnityEngine;

namespace Code.Grid.Form
{
    public class FormConstruct : MonoBehaviour
    {
        [SerializeField] public Grid[] grid;
        [SerializeField] public Vector3 offset;

        [Serializable]
        public class Grid
        {
            [SerializeField] public Transform cell;
            [SerializeField] public Transform[] detect;
            [HideInInspector] public SpriteRenderer[] detectSR;
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

        private Vector3 _last;

        public void LoadView(TypeCell[] views)
        {
            cells = views;
            int i = 0;
            foreach (var tt in cells)
            {
                grid[i].cell.GetComponent<SpriteRenderer>().color = tt.view;
                
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

            foreach (var cell in grid)
            {
                int i = 0;
                cell.detectSR = new SpriteRenderer[cell.detect.Length];
                foreach (var ss in cell.detect)
                {
                    cell.detectSR[i] = ss.GetComponent<SpriteRenderer>();
                    cell.detectSR[i].color = Color.black;
                    ss.gameObject.SetActive(true);

                    i++;
                }
            }
        }

        public bool FindCellReset(Prefab prefab, GameObject eventDataPointerEnter)
        {
            foreach (var cell in grid)
            foreach (var ss in cell.detect)
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

    [Serializable]
    public struct TypeRes
    {
        public int id;
        public Vector2 rangeValue;
    }
}