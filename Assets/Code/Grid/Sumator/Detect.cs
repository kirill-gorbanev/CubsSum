using System;
using System.Collections;
using System.Linq;
using Code.UI.Tools;
using UnityEngine;

namespace Code.Grid.Sumator
{
    [Serializable]
    public class Detect
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TooltipManager tooltipManager;


        public static float CheckAllResource(TypeRes res, Spawner spawner, Action<Vector2Int, float> find)
        {
            var energy = 0f;
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                 

                    if (c.typeCell.res != res)
                        continue;

                    var valR = c.typeCell.res.Range;
                    valR = Sinergy(valR, spawner, c, i, j);

                    spawner.CellsActive[i, j].moment = valR;
                    find?.Invoke(new Vector2Int(i, j), valR);

                    energy += valR;
                }
            }

            return energy;
        }

        public static float UseAllResource(TypeRes useRes, float multUse, Spawner spawner,
            Action<Vector2Int, float> find)
        {
            var energy = 0f;
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                    var e = c.moment;
                  
                    if (e <= 0)
                        continue;
                    if (!c.typeCell.use.Contains(useRes))
                        continue;

                    e = Sinergy(e, spawner, c, i, j);
                    e *= multUse;
                    spawner.CellsActive[i, j].moment = e;
                    find?.Invoke(new Vector2Int(i, j), e);
                    energy += e;
                }
            }

            return energy;
        }

        public static float AllRezult(TypeRes rez, Spawner spawner, Action<Vector2Int, float> find)
        {
            var r = 0f;
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                    var e = c.moment;
                  
                    if (e <= 0)
                        continue;
                    if (c.typeCell.rez != rez)
                        continue;

                    e = Sinergy(e, spawner, c, i, j);
                    spawner.CellsActive[i, j].moment = e;
                    find?.Invoke(new Vector2Int(i, j), e);
                    r += e;
                }
            }

            return r;
        }

        public static float Sinergy(float valR, Spawner spawner, Spawner.GridItem c, int i, int j)
        {
            var g = spawner.grid;

            foreach (var point in c.typeCell.pointers.pointsDetect)
            {
                var d = new Vector2Int((int)point.localPosition.x + i, (int)point.localPosition.y + j);

                if (DetectPos(d, spawner))
                    continue;

                var ce = spawner.CellsActive[d.x, d.y];
          

                foreach (var comp in c.typeCell.compatible)
                {
                    if (comp.id == ce.typeCell)
                    {
                        valR = comp.connect.GetValue(valR);
                    }
                }
            }

            return valR;
        }

        public static bool DetectPos(Vector2Int d, Spawner spawner)
        {
            var g = spawner.grid;
            return d.x < 0 || d.x >= g.x || d.y < 0 || d.y >= g.y;
        }


        private IEnumerator MinAnim(int i, int j, float max)
        {
            var ss = new WaitForSeconds(0.1f);
            if (tooltipManager.tooltips.TryGetValue(new Vector2Int(i, j), out var cellTooltip))
            {
                var t = max;
                while (t > 0)
                {
                    t--;
                    cellTooltip.View($"{t}");
                    yield return ss;
                }

                cellTooltip.InActive();
            }
        }
    }
}