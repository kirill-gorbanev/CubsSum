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

        [SerializeField] private Transform parent;
        [SerializeField] private CellEnergy energyText;


        private void Start()
        {
            spawner.OnLoad += FindEnergy;
        }

        private void FindEnergy()
        {
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];

                    if (c.typeCell.use.Contains(energyType))
                    {
                        var t = Instantiate(energyText, parent);
                        t.gameObject.SetActive(true);
                        t.text.text = c.typeCell.info;
                        var p = new Vector2Int(i, j);
                        t.OnChange += e =>
                        {
                            if (e)
                            {
                            }
                            else
                            {
                            }

                            if (tooltipManager.tooltips.TryGetValue(p, out var cellTooltip))
                                cellTooltip.gameObject.SetActive(true);
                        };
                    }
                }
            }
        }
    }
}