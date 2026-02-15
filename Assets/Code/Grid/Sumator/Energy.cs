using Code.UI;
using Code.UI.Tools;
using TMPro;
using UnityEngine;

namespace Code.Grid.Sumator
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private ItemConfig currentItem;
        [SerializeField] private TooltipManager tooltipManager;
        [SerializeField] private TMP_Text energyText;

        [SerializeField] private TypeRes energyType;

        public float _energy;

        private void Start()
        {
            spawner.OnLoadStep += FindEnergy;
        }

        private void FindEnergy(int step)
        {
            if(step != 1) return;
            
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];

                    var valR = c.typeCell.res.Range;
                    Debug.Log("main " + c.typeCell + " " + new Vector2Int(i, j));
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
                                Debug.Log("mainA " + c.typeCell + "add " + ce.typeCell + " " + d);
                            }
                        }
                    }

                    spawner.CellsActive[i, j].moment = valR;
                    if (tooltipManager.tooltips.TryGetValue(new Vector2Int(i, j), out var tooltip))
                    {
                       // tooltip.gameObject.SetActive(c.typeCell.res == energyType);
                        tooltip.GetComponentInChildren<TMP_Text>().text = valR.ToString();
                    }


                    if (c.typeCell.res == energyType)
                        _energy += valR;
                }
            }

            energyText.text = _energy.ToString();
        }
    }
}