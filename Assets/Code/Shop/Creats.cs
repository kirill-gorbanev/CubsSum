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
        
        YG2.Save.Peres ??= new();
        YG2.Save.Pods ??= new();

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

            c.costValue = item.cost;

         //   var d = dDop * i + damage.y + dX;
        //    damage = new Vector2Int(d, d + dY);
          //  var a = aDop * i + add.y + aX;
          //  add = new Vector2Int(a, a + aY);

            c.cost.text = NumberFormatter.Format(item.cost);
            c.add.text = string.Format(c.formatAdd,  item.add.x, item.add.y);
            c.bgView.color = rangeZones.First(e => e.range == item.range).color;

            var i1 = i;
            c.bt.onClick.AddListener(() =>
            {
                var s = i1 + 1;
                if (_unblocks.Contains(s))
                {
                    if (c.counterCell == null)
                    {
                        YG2.Save.activeIdPods = s;
                        Compl(item, s,   item.add, c.bgCell);
                    }
                    else
                    {
                        if (!c.counterCell.IsMax(s))
                            if (OnBye != null && OnBye.Invoke(c.costValue))
                            {
                                YG2.Save.Peres ??= new();
                                var cs = YG2.Save.Peres.FirstOrDefault(e => e.id == s);
                                if (cs == null)
                                {
                                    cs = new As { id = s, count = 0 };
                                    YG2.Save.Peres.Add(cs);
                                }

                                cs.count++;

                                c.counterCell.View(s);
                                c.costValue *= 2;
                                c.cost.text = c.costValue.ToString();

                                Compl(item, s,  item.add, c.bgCell);
                            }
                    }
                }
                else if (OnBye != null && OnBye.Invoke(c.costValue))
                {
                    YG2.Save.Pods.Add(s);
                    Compl(item, s,   item.add, c.bgCell);
                    c.Active();
                    if (c.counterCell == null)
                        YG2.Save.activeIdPods = s;
                    else
                    {
                        var cs = YG2.Save.Peres.FirstOrDefault(e => e.id == s);
                        if (cs == null)
                            YG2.Save.Peres.Add(new As { id = s, count = 1 });
                        else
                            cs.count++;
                    }
                }
            });


            i++;

            if (c.counterCell != null && YG2.Save.Peres != null)
            {
                var s = YG2.Save.Peres.FirstOrDefault(e => e.id == i);
                if (s == null)
                {
                    s = new As { id = i, count = 0 };
                    YG2.Save.Peres.Add(s);
                }
                else
                    _unblocks.Add(i);

                c.counterCell.id = YG2.Save.Peres.IndexOf(s);

                for (int j = 0; j < s.count; j++)
                {
                    c.counterCell.View(i);
                    c.costValue *= 2;
                    c.cost.text = c.costValue.ToString();

                    Compl(item, YG2.Save.activeIdPods,   item.add, c.bgCell);
                }
            }


            if (c.counterCell == null && YG2.Save.Pods != null && YG2.Save.Pods.Count > 0)
            {
                var vv = YG2.Save.Pods.FirstOrDefault(e => e == i);
                if (vv != 0)
                {
                    _unblocks.Add(i);
                    c.Active();
                }
            }

            if (i == YG2.Save.activeIdPods && c.counterCell == null)
                Compl(item, i,   item.add, c.bgCell);
        }
    }

    private void Compl(Item item, int step, Vector2Int add1, Image c)
    {
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