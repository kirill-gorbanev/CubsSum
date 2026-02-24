using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.UI;
using Code.UI.Tools;
using UnityEngine;

namespace Code.Grid.Sumator
{
    public class UseEnergy : MonoBehaviour
    {
        [SerializeField] private ItemConfig currentItem;
        [SerializeField] private TypeRes energyType;
        [SerializeField] private Spawner spawner;
        [SerializeField] private TooltipManager tooltipManager;
        [SerializeField] private Energy energy;

        [SerializeField] private Transform parent;
        [SerializeField] private CellEnergy energyText;

        private float _cur;

        private void Start()
        {
            spawner.OnLoadStep += FindEnergy;
        }

        private List<Vector2Int> _energys = new();

        private void FindEnergy(int step)
        {
            if (step != 2) return;

            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];

                    var p =new Vector2Int(i, j) ;
                    if (c.typeCell.res == energyType)
                        _energys.Add(p);

                    if (c.typeCell.use.Contains(energyType))
                    {
                        
                        if (tooltipManager.tooltips.TryGetValue(p, out var cc))
                            cc.View(c.moment.ToString());

                        var t = Instantiate(energyText, parent);
                        t.gameObject.SetActive(true);
                        t.text.text = c.typeCell.info;


                        t.find.onClick.AddListener(() =>
                        {
                            if (tooltipManager.tooltips.TryGetValue(p, out var cellTooltip))
                                cellTooltip.Find();
                        });
                        if (tooltipManager.tooltips.TryGetValue(p, out var cellTooltip))
                            cellTooltip.Sub(() => { t.OnChange?.Invoke(true); });

                        t.OnChange += e =>
                        {
                            var d = Add(p, e);

                            if (tooltipManager.tooltips.TryGetValue(p, out var cellTooltip))
                            {
                                cellTooltip.View(d.ToString());
                                t.text.text = c.typeCell.info + $" {d}";
                                t.bg.color = d > 0 ? Color.green : Color.white;
                            }
                        };
                    }
                }
            }

            StartCoroutine(UseEnergyAnim());
        }

        private IEnumerator UseEnergyAnim()
        {
            var s = new WaitForSeconds(0.05f);

            foreach (var pp in _energys)
            {
                if (tooltipManager.tooltips.TryGetValue(pp, out var cellTooltip))
                {
                    var e = spawner.CellsActive[pp.x, pp.y].moment;
                    while (e >= 0)
                    {
                        e--;
                        cellTooltip.View($"{e}");
                        yield return s;
                    }

                    spawner.CellsActive[pp.x, pp.y].moment = 0;
                    cellTooltip.InActive();
                }
            }
        }

        private float Add(Vector2Int p, bool isAdd)
        {
            var c = spawner.CellsActive[p.x, p.y];
            int delta = isAdd ? 1 : -1;

            float newMoment = c.moment + delta;
            float newCur = _cur + delta;

            if (newCur < 0 || newCur > energy._energy || newMoment < 0 || newMoment >= energy._energy)
                return c.moment;

            c.moment = newMoment;
            spawner.CellsActive[p.x, p.y] = c;
            _cur = newCur;

            return newMoment;
        }
    }
}