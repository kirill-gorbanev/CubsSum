using System;
using System.Collections.Generic;
using System.Linq;
using Code.Shop;
using UnityEngine;
using UnityEngine.UI;
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

            var v = Object.Instantiate(item.view, c.parentView);
            v.transform.localScale *= item.multSizeView;
            v.transform.localPosition += item.offsetPos;
            v.transform.localRotation = Quaternion.Euler(item.rot);

            c.gameObject.SetActive(true);

            var cost = config.costInit * (i + 1);
            var d = dDop * i + damage.y + dX;
            damage = new Vector2Int(d, d + dY);
            var a = aDop * i + add.y + aX;
            add = new Vector2Int(a, a + aY);

            c.cost.text = cost.ToString();
            if (c.damage != null)
                c.damage.text = $"{damage.x}-{damage.y}";
            c.add.text = string.Format(c.formatAdd, add.x, add.y);
            c.bgView.color = rangeZones.First(e => e.range == item.range).color;

            var i1 = i;
            var damage1 = damage;
            var add1 = add;
            c.bt.onClick.AddListener(() =>
            {
                var step = i1 + 1;

                if (_unblocks.Contains(step))
                {
                    Compl(item, step, damage1, add1, c.bgCell);
                }

                else if (OnBye != null && OnBye.Invoke(cost))
                {
                    Compl(item, step, damage1, add1, c.bgCell);
                }
            });

            i++;
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

        if(last != null)
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