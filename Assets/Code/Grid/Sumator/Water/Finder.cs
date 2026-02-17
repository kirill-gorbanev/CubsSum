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

                waterDetect.Check();
                waterText.text = waterDetect._energy.ToString();
                desertDetect.Check();
                desertText.text = desertDetect._energy.ToString();

                if (waterDetect._energy > desertDetect._energy)
                {
                    Forest(waterDetect._energy - desertDetect._energy);
                    desertDetect._energy = 0;
                    waterDetect._energy = 0;

                    var g = spawner.grid;
                    for (int i = 0; i < g.x; i++)
                    {
                        for (int j = 0; j < g.y; j++)
                        {
                            var c = spawner.CellsActive[i, j];

                            if (c.typeCell.res == desertType)
                                spawner.CellsActive[i, j].moment = 0;
                        }
                    }
                }
                else
                {
                    desertDetect._energy -= waterDetect._energy;
                    waterDetect._energy = 0;

                    var g = spawner.grid;
                    for (int i = 0; i < g.x; i++)
                    {
                        for (int j = 0; j < g.y; j++)
                        {
                            var c = spawner.CellsActive[i, j];

                            if (c.typeCell.res == waterType)
                                spawner.CellsActive[i, j].moment = 0;
                        }
                    }

                    happy.Damage(desertDetect._energy);
                }

                waterText.text = waterDetect._energy.ToString();
                desertText.text = desertDetect._energy.ToString();
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
                        spawner.CellsActive[i, j].moment += water;
                    if (c.typeCell.res == waterType)
                        spawner.CellsActive[i, j].moment = 0;
                }
            }
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