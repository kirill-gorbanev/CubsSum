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
            for (int j = 0; j < g.y; j++)
                if (tooltipManager.tooltips.TryGetValue(new Vector2Int(i, j), out var us))
                    us.GetComponentInChildren<TMP_Text>().text = spawner.CellsActive[i, j].moment.ToString();

            if (step != 3) return;


            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                    float e = c.moment;

                    if (e <= 0) continue;
                    if (c.typeCell.rez != waterType) continue;

                    bool iss = false;
                    foreach (var point in c.typeCell.pointers.pointsDetect)
                    {
                        var w = new Vector2Int((int)point.localPosition.x + i, (int)point.localPosition.y + j);
                        if (Detect(w))
                            continue;

                        var cc = spawner.CellsActive[w.x, w.y];
                        if (cc.typeCell.res != waterType)
                            continue;

                        spawner.CellsActive[w.x, w.y].moment += e;
                        iss = true;
                    }

                    if (iss)
                        spawner.CellsActive[i, j].moment = 0;
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