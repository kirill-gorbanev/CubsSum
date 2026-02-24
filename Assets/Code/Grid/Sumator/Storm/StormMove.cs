using System.Collections;
using System.Linq;
using Code.UI.Tools;
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
        [SerializeField] private SpriteRenderer srPrefabStorm;

        [SerializeField] private TypeCell[] stopStorm;
        [SerializeField] private TypeRes[] damageStorm;
        [SerializeField] private TooltipManager tooltipManager;

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
                if (step != 5) return;

                var g = spawner.grid;
                for (int j = 0; j < g.y; j++)
                {
                    var ePos = spawner.transform.position + (Vector3)(new Vector2(isRight ? 0 : g.x, j) * spawner.size);
                    var p = Instantiate(srPrefabStorm, ePos, Quaternion.identity);
                    var d = damage * g.x;
                    if (isRight)
                        for (int i = 0; i < g.x; i++)
                        {
                            d = Damage(i, j, d);
                            if (d <= 0)
                            {
                                ePos = spawner.transform.position + (Vector3)(new Vector2(i, j) * spawner.size);
                                break;
                            }
                        }

                    else
                        for (int i = g.x - 1; i >= 0; i--)
                        {
                            d = Damage(i, j, d);
                            if (d <= 0)
                            {
                                ePos = spawner.transform.position + (Vector3)(new Vector2(i, j) * spawner.size);
                                break;
                            }
                        }

                    if (d > 0)
                        ePos = spawner.transform.position + (Vector3)(new Vector2(isRight ? g.x : 0, j) * spawner.size);
                    StartCoroutine(Move(p.transform, ePos));
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
                    var r = spawner.CellsActive[x, y].moment -= damage;
                    StartCoroutine(SpineTextMin(new Vector2Int(x, y), e, r));
                    return -1;
                }
                else
                {
                    spawner.CellsActive[x, y].moment = 0;
                    StartCoroutine(SpineTextMin(new Vector2Int(x, y), e, 0));
                    return damage - e;
                }
            }

            if (damageStorm.Contains(c.typeCell.rez))
            {
                if (e >= damage)
                {
                    var r = spawner.CellsActive[x, y].moment -= damage;
                    StartCoroutine(SpineTextMin(new Vector2Int(x, y), e, r));
                    return -1;
                }
                else
                {
                    spawner.CellsActive[x, y].moment = 0;
                    StartCoroutine(SpineTextMin(new Vector2Int(x, y), e, 0));
                    return damage - e;
                }
            }

            return damage;
        }

        private IEnumerator SpineTextMin(Vector2Int p, float max, float min)
        {
            var ss = new WaitForSeconds(0.05f);
            if (tooltipManager.tooltips.TryGetValue(p, out var us))
            {
                var t = max;
                while (t > min)
                {
                    t--;
                    us.View(t.ToString());
                    yield return ss;
                }

                us.View(min.ToString());
            }
        }

        private IEnumerator Move(Transform target, Vector2 end)
        {
            while (Vector2.Distance(target.position, end) > 0.1f)
            {
                target.position = Vector2.MoveTowards(target.position, end, Time.deltaTime * 10f);
                yield return null;
            }

            Destroy(target.gameObject);
        }
    }
}