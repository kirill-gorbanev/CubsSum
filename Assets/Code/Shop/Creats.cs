using System;
using System.Collections.Generic;
using System.Linq;
using Code.Shop;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Range = Code.Shop.Range;

[Serializable]
public class Creats
{
    [SerializeField] private ContentConfig config;
    [SerializeField] private CellShop cellShop;
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform parentView;
    [SerializeField] private Color active;
    [SerializeField] private Color inactive;

    [HideInInspector] public Vector2Int _rangeCost;
    private Vector2Int _rangeTox;
    private Range _range;
    [HideInInspector] public GameObject view;

    public event Func<int, bool> OnBye;
    public event Action OnComplete;

    private HashSet<int> _unblocks = new();
    private Image last;
    public int _step;

    public List<CellShop> _cells = new();

    public void Start()
    {
        if (config == null)
            return;

        int i = 0;

        var dX = config.rangeInitDamage.x;
        var aX = config.rangeInitAdd.x;
        var dY = config.rangeInitDamage.y;
        var aY = config.rangeInitAdd.y;
        var dDop = config.dopDamage;
        var aDop = config.dopAdd;
        var damage = config.rangeInitDamage;
        var add = config.rangeInitAdd;

        foreach (Item item in config.items)
        {
            var c = Object.Instantiate(cellShop, parent);
            c.rectTransform = parentView;
            _cells.Add(c);

            var v = Object.Instantiate(item.view, c.parentView);
            v.transform.localScale *= item.multSizeView;
            v.transform.localPosition += item.offsetPos;
            v.transform.localRotation = Quaternion.Euler(item.rot);

            c.gameObject.SetActive(true);

            var cost = config.costInit * (i + 1);
            c.costValue = cost;

            var d = dDop * i + damage.y + dX;
            damage = new Vector2Int(d, d + dY);
            var a = aDop * i + add.y + aX;
            add = new Vector2Int(a, a + aY);

            c.cost.text = cost.ToString();
            c.add.text = string.Format(c.formatAdd, add.x, add.y);
            c.bgView.color = rangeZones.First(e => e.range == item.range).color;

            var i1 = i;
            var damage1 = damage;
            var add1 = add;
            c.bt.onClick.AddListener(() =>
            {
                var s = i1 + 1;
                if (_unblocks.Contains(s))
                {
                    if (c.counterCell == null)
                    {
                        YG2.saves.activeIdPods = s;
                        Compl(item, s, damage1, add1, c.bgCell);
                    }
                    else
                    {
                        if (!c.counterCell.IsMax)
                            if (OnBye != null && OnBye.Invoke(c.costValue))
                            {
                                var cs = YG2.saves.Pers.FirstOrDefault(e => e.id == s);
                                if (cs == null)
                                    YG2.saves.Pers.Add(new As { id = s, count = 2 });
                                else
                                    cs.count++;

                                c.counterCell.View();
                                c.costValue *= 2;
                                c.cost.text = c.costValue.ToString();

                                Compl(item, s, damage1, add1, c.bgCell);
                            }
                    }
                }
                else if (OnBye != null && OnBye.Invoke(c.costValue))
                {
                    YG2.saves.pods.Add(s);
                    Compl(item, s, damage1, add1, c.bgCell);
                    c.Active();
                    if (c.counterCell == null)
                        YG2.saves.activeIdPods = s;
                    else
                    {
                        var cs = YG2.saves.Pers.FirstOrDefault(e => e.id == s);
                        if (cs == null)
                            YG2.saves.Pers.Add(new As { id = s, count = 1 });
                        else
                            cs.count++;
                    }
                }
            });


            i++;

            if (c.counterCell != null)
            {
                var s = YG2.saves.Pers.FirstOrDefault(e => e.id == i);
                if (s == null)
                {
                    s = new As { id = i, count = 0 };
                    YG2.saves.Pers.Add(s);
                }
                else
                    _unblocks.Add(i);

                c.counterCell.id = YG2.saves.Pers.IndexOf(s);

                for (int j = 0; j < s.count; j++)
                {
                    c.counterCell.View();
                    c.costValue *= 2;
                    c.cost.text = c.costValue.ToString();

                    Compl(item, YG2.saves.activeIdPods, damage1, add1, c.bgCell);
                }
            }

            if (YG2.saves.pods.Contains(i) && c.counterCell == null)
            {
                _unblocks.Add(i);
                c.Active();
            }

            if (i == YG2.saves.activeIdPods && c.counterCell == null)
                Compl(item, i, damage1, add1, c.bgCell);
        }
    }

    private void Compl(Item item, int step, Vector2Int damage1, Vector2Int add1, Image c)
    {
        _rangeTox = damage1;
        _rangeCost = add1;
        _step = step;
        _unblocks.Add(_step);
        _range = item.range;
        view = item.view;
        //    c.gameObject.SetActive(false);
        OnComplete?.Invoke();

        if (last != null)
            last.color = inactive;
        c.color = active;
        last = c;
    }

    [Serializable]
    public struct RangeZone
    {
        public Range range;
        public Vector2 speed;
        public Vector2 size;
        public Color color;
    }

    [SerializeField] private RangeZone[] rangeZones;

    public (int coinMult, int toxMult, float size, float speed) GetStep()
    {
        var s = rangeZones.First(e => e.range == _range);
        return (Random.Range(_rangeCost.x, _rangeCost.y),
            Random.Range(_rangeTox.x, _rangeTox.y),
            Random.Range(s.size.x, s.size.y),
            Random.Range(s.speed.x, s.speed.y));
    }
}