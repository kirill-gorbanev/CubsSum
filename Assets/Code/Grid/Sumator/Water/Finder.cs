using System.Collections;
using Code.Parametars;
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
        [SerializeField] private TypeRes desertType;
        [SerializeField] private Detect waterDetect;
        [SerializeField] private TMP_Text waterText;
        [SerializeField] private Detect desertDetect;
        [SerializeField] private TMP_Text desertText;
        [SerializeField] private Happy happy;
        [SerializeField] private TypeRes forest;

        private void Start()
        {
            spawner.OnLoadStep += Find;
            spawner.OnLoadStep += (step) =>
            {
                if (step != 4) return;

                var allWater = Detect.CheckAllResource(waterType, spawner,
                    (p, e) => { StartCoroutine(SpineTextMin(p, e)); });
                StartCoroutine(SpineText(waterText, allWater));

                var allDesert = Detect.CheckAllResource(desertType, spawner,
                    (p, e) => { StartCoroutine(SpineTextMin(p, e)); });
                StartCoroutine(SpineText(desertText, allDesert));

                if (allWater > allDesert)
                {
                    Forest(allWater - allDesert);
                    allDesert = 0;
                    allWater = 0;
                }
                else
                {
                    allDesert -= allWater;
                    allWater = 0;
                    happy.Damage(allDesert);
                }

                waterText.text = allWater.ToString();
                desertText.text = allDesert.ToString();
            };
        }

        private void Forest(float water)
        {
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                
                    if (c.typeCell.res == forest)
                    {
                        var v = spawner.CellsActive[i, j].moment += water;
                        StartCoroutine(SpineText(new Vector2Int(i, j), v));
                    }
                }
            }
        }

        private void Find(int step)
        {
            if (step != 3) return;

            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
               
               

                    if (c.typeCell.rez != waterType) continue;

                    float e = c.moment;
                    if (e <= 0)
                    {
                        if (tooltipManager.tooltips.TryGetValue(new Vector2Int(i, j), out var tooltip))
                            tooltip.InActive();

                        continue;
                    }

                    bool iss = false;
                    foreach (var point in c.typeCell.pointers.pointsDetect)
                    {
                        var w = new Vector2Int((int)point.localPosition.x + i, (int)point.localPosition.y + j);
                        if (Detect1(w))
                            continue;

                        var cc = spawner.CellsActive[w.x, w.y];
                        if (cc.typeCell.res != waterType)
                            continue;

                        var v = spawner.CellsActive[w.x, w.y].moment += e;
                        StartCoroutine(SpineText(w, v));
                        iss = true;
                    }

                    if (iss)
                    {
                        StartCoroutine(SpineTextMin(new Vector2Int(i, j), spawner.CellsActive[i, j].moment));
                        spawner.CellsActive[i, j].moment = 0;
                    }
                }
            }
        }

        private IEnumerator SpineText(Vector2Int p, float value)
        {
            var ss = new WaitForSeconds(0.05f);
            if (tooltipManager.tooltips.TryGetValue(p, out var us))
            {
                var t = 0;
                while (t <= value)
                {
                    t++;
                    us.View(t.ToString());
                    yield return ss;
                }
            }
        }

        private IEnumerator SpineText(TMP_Text us, float value)
        {
            var ss = new WaitForSeconds(0.05f);
            {
                var t = 0;
                while (t <= value)
                {
                    t++;
                    us.text = t.ToString();
                    yield return ss;
                }
            }
        }

        private IEnumerator SpineTextMin(Vector2Int p, float value)
        {
            var ss = new WaitForSeconds(0.05f);
            if (tooltipManager.tooltips.TryGetValue(p, out var us))
            {
                var t = value;
                while (t > 0)
                {
                    t--;
                    us.View(t.ToString());
                    yield return ss;
                }
                us.InActive();
            }
        }

        private bool Detect1(Vector2Int d)
        {
            var g = spawner.grid;
            return d.x < 0 || d.x >= g.x || d.y < 0 || d.y >= g.y;
        }
    }
}