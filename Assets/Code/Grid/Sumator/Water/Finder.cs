using System.Linq;
using Code.UI.Tools;
using TMPro;
using UnityEngine;

namespace Code.Grid.Sumator.Water
{
    public class Finder : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TooltipManager tooltipManager;

        [SerializeField] private TypeRes waterType;

        private void Start()
        {
            spawner.OnLoadStep += Find;
        }

        private void Find(int step)
        {
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                    float e = c.moment;

                    if (tooltipManager.tooltips.TryGetValue(new Vector2Int(i, j), out var us))
                    {
                        us.GetComponentInChildren<TMP_Text>().text = e.ToString();
                    }
                }
            }

            if (step != 2) return;


            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                    float e = c.moment;

                    if (e <= 0) continue;
                    if (c.typeCell.rez != waterType) continue;


                    foreach (var point in c.typeCell.pointers.pointsDetect)
                    {
                        var w = new Vector2Int((int)point.localPosition.x + i, (int)point.localPosition.y + j);
                        if (Detect(w))
                            continue;

                        var cc = spawner.CellsActive[w.x, w.y];
                        if (cc.typeCell.res != waterType)
                            continue;
                        Debug.Log(w);

                        var valR = cc.typeCell.res.Range;
                        foreach (var pointW in c.typeCell.pointers.pointsDetect)
                        {
                            var d = new Vector2Int((int)pointW.localPosition.x + w.x, (int)pointW.localPosition.y + w.y);
                            if (Detect(d))
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

                        cc.moment = valR + e;

                        spawner.CellsActive[w.x, w.y] = cc;

                        if (tooltipManager.tooltips.TryGetValue(w, out var tooltip))
                        {
                            tooltip.gameObject.SetActive(true);
                            tooltip.GetComponentInChildren<TMP_Text>().text = cc.moment.ToString();
                        }
                    }

                    c.moment = 0;
                    spawner.CellsActive[i, j] = c;

                    if (tooltipManager.tooltips.TryGetValue(new Vector2Int(i, j), out var u))
                    {
                        u.GetComponentInChildren<TMP_Text>().text = "0";
                    }
                }
            }
        }

        private bool Detect(Vector2Int d)
        {
            var g = spawner.grid;
            return d.x < 0 || d.x >= g.x || d.y < 0 || d.y >= g.y;
        }
    }
}