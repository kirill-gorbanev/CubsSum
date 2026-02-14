using System.Linq;
using Code.UI;
using Code.UI.Tools;
using TMPro;
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
                            var d = Add(p, e);

                            if (tooltipManager.tooltips.TryGetValue(p, out var cellTooltip))
                            {
                                cellTooltip.gameObject.SetActive(true);
                                cellTooltip.GetComponentInChildren<TMP_Text>().text = d.ToString();
                            }
                        };
                    }
                }
            }
        }

        private float Add(Vector2Int p, bool isAdd)
        {
            var c = spawner.CellsActive[p.x, p.y];
            int delta = isAdd ? 1 : -1;

            // Проверяем будущее состояние ДО изменения
            float newMoment = c.moment + delta;
            float newCur = _cur + delta;

            // Валидация границ
            if (newCur < 0 || newCur > energy._energy ||
                newMoment < 0 || newMoment >= energy._energy)
            {
                return c.moment; // Отменяем операцию, возвращаем текущее значение
            }

            // Применяем изменения
            c.moment = newMoment;
            spawner.CellsActive[p.x, p.y] = c;
            _cur = newCur;

            return newMoment;
        }
    }
}