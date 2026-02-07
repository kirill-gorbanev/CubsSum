using System;
using Code.Grid.Form;
using UnityEngine;

namespace Code.Grid
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Vector2Int grid;
        [SerializeField] public Vector2 size;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color a;
        [SerializeField] private Color b;

        [SerializeField] private Camera mainCamera;

        private bool[,] cellsActive;

        public event Action<ItemInfo> OnNewSpawn;

        public struct ItemInfo
        {
            public Vector2Int id;
            public Vector2 pos;
            public bool isMain; // <- isGroup
        }

        private void Start()
        {
            cellsActive = new bool[grid.x, grid.y];

            for (int i = 0; i < grid.x; i++)
            {
                for (int j = 0; j < grid.y; j++)
                {
                    var sr = Instantiate(spriteRenderer, (Vector2)transform.position + new Vector2(i, j) * size,
                        Quaternion.identity);
                    sr.name = i + "-" + j;
                    sr.transform.localScale = size;
                    if ((j + (i % 2 == 0 ? 1 : 0)) % 2 == 0)
                        sr.color = a;
                    else
                        sr.color = b;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (cellsActive == null)
            {
                for (int i = 0; i < grid.x; i++)
                for (int j = 0; j < grid.y; j++)
                    Gizmos.DrawCube((Vector2)transform.position + new Vector2(i, j) * size, size- Vector2.one * 0.05f);

                return;
            }

            for (int i = 0; i < grid.x; i++)
            {
                for (int j = 0; j < grid.y; j++)
                {
                    Gizmos.color = cellsActive[i, j] ? Color.green : Color.gray;
                    Gizmos.DrawCube((Vector2)transform.position + new Vector2(i, j) * size, size);
                }
            }
        }

        public ItemInfo[] Connect(FormConstruct construct)
        {
            var m = GetGridCellUnderMouse();
            ItemInfo[] rez = new ItemInfo[construct.grid.Length];
            int i = 0;
            foreach (var grs in construct.grid)
            {
                var gp = grs.position - construct.transform.position + (Vector3)size / 2f;

                int cellX = Mathf.FloorToInt(gp.x / size.x);
                int cellY = Mathf.FloorToInt(gp.y / size.y);

                var d = new Vector2Int(cellX, cellY) + m;
                Debug.Log(d);

                if (d.x < 0 || d.x >= grid.x || d.y < 0 || d.y >= grid.y)
                    return null;

                if (cellsActive[d.x, d.y])
                    return null;

                rez[i++] = new ItemInfo
                    { id = d, pos = (Vector2)transform.position + new Vector2(d.x, d.y) * size, isMain = true };
            }

            return rez;
        }

        public void SetPos(ItemInfo[] poss)
        {
            foreach (var d in poss)
            {
                cellsActive[d.id.x, d.id.y] = true;
                OnNewSpawn?.Invoke(d);
            }
        }

        private Vector2Int GetGridCellUnderMouse()
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position +
                                    (Vector3)size / 2;

            int cellX = Mathf.FloorToInt(mouseWorldPos.x / size.x);
            int cellY = Mathf.FloorToInt(mouseWorldPos.y / size.y);

            return new Vector2Int(cellX, cellY);
        }
    }
}