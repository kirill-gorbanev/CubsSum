using System;
using System.Collections;
using Code.UI.Tools;
using UnityEngine;

namespace Code.Grid.Sumator
{
    [Serializable]
    public class Detect
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TypeRes energyType;
        [SerializeField] private TooltipManager tooltipManager;

        public float _energy { get; set; }


        public void Check()
        {
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];

                    var valR = c.typeCell.res.Range;
                    foreach (var point in c.typeCell.pointers.pointsDetect)
                    {
                        var d = new Vector2Int((int)point.localPosition.x + i, (int)point.localPosition.y + j);

                        if (d.x < 0 || d.x >= g.x || d.y < 0 || d.y >= g.y)
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

                    if (c.typeCell.res == energyType)
                    {
                        _energy += valR;
                        if (tooltipManager.tooltips.TryGetValue(new Vector2Int(i, j), out var cellTooltip))
                            cellTooltip.View($"{valR}");
                    }

                    spawner.CellsActive[i, j].moment = valR;
                }
            }
        }

        public void CheckZero(MonoBehaviour monoBehaviour)
        {
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];

                    var valR = c.typeCell.res.Range;
                    foreach (var point in c.typeCell.pointers.pointsDetect)
                    {
                        var d = new Vector2Int((int)point.localPosition.x + i, (int)point.localPosition.y + j);

                        if (d.x < 0 || d.x >= g.x || d.y < 0 || d.y >= g.y)
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

                    if (c.typeCell.res == energyType)
                    {
                        _energy += valR;
                        monoBehaviour.StartCoroutine(MinAnim(i, j, valR));
                    }
                    else
                        spawner.CellsActive[i, j].moment = valR;
                }
            }
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