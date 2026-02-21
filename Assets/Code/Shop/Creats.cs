using System;
using System.Linq;
using Code.Shop;
using UnityEngine;
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

    [SerializeField] private int stepCost;
    [SerializeField] private int initCost;

    private Vector2Int _rangeCost;
    private Vector2Int _rangeTox;
    private Range _range;

    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 speedRange;

    public event Func<int, bool> OnBye;
    public event Action OnComplete;

    private int _step;

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

        foreach (var item in config.items)
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
            c.damage.text = $"{damage.x}-{damage.y}";
            c.add.text = $"{add.x}-{add.y}";
            c.bgView.color = rangeZones.First(e => e.range == item.range).color;

            var i1 = i;
            var damage1 = damage;
            var add1 = add;
            c.bt.onClick.AddListener(() =>
            {
                if (OnBye != null && OnBye.Invoke(cost))
                {
                    _rangeTox = damage1;
                    _rangeCost = add1;
                    _step = i1 + 1;
                    _range = item.range;
                    c.gameObject.SetActive(false);
                    OnComplete?.Invoke();
                }
            });

            i++;
        }
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