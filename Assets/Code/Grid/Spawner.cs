using System;
using System.Linq;
using Code.Grid.Form;
using Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Grid
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] public Vector2Int grid;
        [SerializeField] public Vector2 size;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color a;
        [SerializeField] private Color b;

        [SerializeField] private Camera mainCamera;

        [SerializeField] private Button nextAge;
        [SerializeField] private ItemConfig itemConfig;
        [SerializeField] private TypeCell idAlls;
        [SerializeField] private FormConstruct prefab;
        
        public GridItem[,] CellsActive { get; private set; }

        public event Action<ItemInfo> OnNewSpawn;
        public event Action OnLoad;

        public struct GridItem
        {
            public bool active;
            public TypeCell typeCell;
        }

        public struct ItemInfo
        {
            public Vector2Int id;
            public Vector2 pos;
            public bool isMain;
            public TypeCell typeCell;
        }

        private void Awake()
        {
            nextAge.onClick.AddListener(() =>
            {
                for (int i = 0; i < grid.x; i++)
                {
                    for (int j = 0; j < grid.y; j++)
                    {
                        var c = CellsActive[i, j];
                        if (!c.active)
                        {
                            c.active = true;
                            c.typeCell = idAlls;
                            CellsActive[i, j] = c;

                            var p = Instantiate(prefab, (Vector2)transform.position + new Vector2(i, j) * size,
                                Quaternion.identity);
                            p.transform.localScale = size;

                            p.LoadView(new[] { idAlls });
                            OnNewSpawn?.Invoke(
                                new ItemInfo
                                {
                                    id = new Vector2Int(i, j),
                                    pos = (Vector2)transform.position + new Vector2(i, j) * size,
                                    isMain = true,
                                    typeCell = idAlls
                                });
                        }
                    }
                }

                OnLoad?.Invoke();
                nextAge.enabled = false;
            });
        }

        private void Start()
        {
            CellsActive = new GridItem[grid.x, grid.y];

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
            if (CellsActive == null)
            {
                for (int i = 0; i < grid.x; i++)
                for (int j = 0; j < grid.y; j++)
                    Gizmos.DrawCube((Vector2)transform.position + new Vector2(i, j) * size, size - Vector2.one * 0.05f);

                return;
            }

            for (int i = 0; i < grid.x; i++)
            {
                for (int j = 0; j < grid.y; j++)
                {
                    Gizmos.color = CellsActive[i, j].active ? Color.green : Color.gray;
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
                var d = GetPos(construct.transform, grs.cell.position, m);

                if (d == null || CellsActive[d.Value.x, d.Value.y].active)
                    return null;

                rez[i] = new ItemInfo
                {
                    id = d.Value,
                    pos = (Vector2)transform.position + new Vector2(d.Value.x, d.Value.y) * size,
                    isMain = !construct.IsGroup || i == 0,
                    typeCell = construct.cells[i]
                };


                i++;
            }

            return rez;
        }

        public void Detect(FormConstruct construct)
        {
            var m = GetGridCellUnderMouse();
            int i = 0;
            foreach (var grs in construct.grid)
            {
                int j = 0;
                foreach (var item in grs.detectSR)
                {
                    var d = GetPos(construct.transform, item.transform.position, m);
                    if (d != null)
                    {
                        var v = CellsActive[d.Value.x, d.Value.y];
                        var s = v.active && itemConfig.GridCont[construct.cells[i]].Any(e => e.id == v.typeCell);

                        construct.Preview(i, j, s);
                    }

                    j++;
                }

                i++;
            }
        }

        private Vector2Int? GetPos(Transform construct, Vector3 pos, Vector2Int m)
        {
            var gp = pos - construct.transform.position + (Vector3)size / 2f;

            int cellX = Mathf.FloorToInt(gp.x / size.x);
            int cellY = Mathf.FloorToInt(gp.y / size.y);

            var d = new Vector2Int(cellX, cellY) + m;

            if (d.x < 0 || d.x >= grid.x || d.y < 0 || d.y >= grid.y)
                return null;

            return d;
        }

        public void SetPos(ItemInfo[] poss)
        {
            foreach (var d in poss)
            {
                CellsActive[d.id.x, d.id.y] = new GridItem
                {
                    active = true,
                    typeCell = d.typeCell
                };

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