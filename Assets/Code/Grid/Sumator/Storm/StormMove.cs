using System.Linq;
using TMPro;
using UnityEngine;

namespace Code.Grid.Sumator.Storm
{
    public class StormMove : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private bool isRight;
        [SerializeField] private Spawner spawner;
        [SerializeField] private TMP_Text damageTx;
        [SerializeField] private RectTransform parent;

        [SerializeField] private TypeCell[] stopStorm;
        [SerializeField] private TypeRes[] damageStorm;

        private void Start()
        {
            var v = spawner.grid.x * spawner.grid.y * damage;
            damageTx.text = v.ToString();

            var pos = (Vector2)spawner.transform.position + new Vector2(0, spawner.grid.y / 2f) * spawner.size;
            if (!isRight)
                pos = (Vector2)spawner.transform.position +
                      new Vector2(spawner.grid.x, spawner.grid.y / 2f) * spawner.size;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, Camera.main,
                    out Vector2 localPoint))
                return;

            var rectTransform = damageTx.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = localPoint;

            spawner.OnLoadStep += step =>
            {
                if (step != 6) return;
                
                var g = spawner.grid;
                for (int j = 0; j < g.y; j++)
                {
                    var d = damage *  g.x;
                    if (isRight)
                        for (int i = 0; i < g.x; i++)
                        {
                            d = Damage(i, j, d);
                            if (d <= 0)
                                break;
                        }

                    else
                        for (int i = g.x - 1; i >= 0; i--)
                        {
                            d = Damage(i, j, d);
                            if (d <= 0)
                                break;
                        }
                }
            };
        }

        private float Damage(int x, int y, float damage)
        {
            var c = spawner.CellsActive[x, y];
            var e = c.moment;

            if (stopStorm.Contains(c.typeCell))
                return -1;
            
            if (e <= 0)
                return damage;
            
            if (damageStorm.Contains(c.typeCell.res))
            {
                if (e >= damage)
                {
                    spawner.CellsActive[x, y].moment -= e;
                    return -1;
                }
                else
                {
                    spawner.CellsActive[x, y].moment = 0;
                    return damage - e;
                }
            }

            return damage;
        }
    }
}