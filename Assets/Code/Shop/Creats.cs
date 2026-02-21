using System;
using Code.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class Creats
{
    [SerializeField] private ContentConfig config;
    [SerializeField] private CellShop cellShop;
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform parentView;

    [SerializeField] private int stepCost;
    [SerializeField] private int initCost;

    [SerializeField] private Vector2Int rangeCost;
    [SerializeField] private Vector2Int rangeTox;
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 speedRange;

    public event Func<int, bool> OnBye;

    private int _step;

    public void Start()
    {
        if (config == null)
            return;

        int i = 0;
        foreach (var item in config.items)
        {
            var c = Object.Instantiate(cellShop, parent);
            c.rectTransform = parentView;
            
            var v = Object.Instantiate(item.view, c.parentView);
            v.transform.localScale *= item.multSizeView;
            v.transform.localPosition += item.offsetPos;
            v.transform.localRotation = Quaternion.Euler(item.rot);

            c.gameObject.SetActive(true);
            var cost = item.cost;
            c.GetComponentInChildren<TMP_Text>().text = cost.ToString();
            var i1 = i;
            c.bt.onClick.AddListener(() =>
            {
                if (OnBye != null && OnBye.Invoke(cost))
                {
                    _step = i1 + 1;
                    c.gameObject.SetActive(false);
                }
            });
        }
    }

    public (int coinMult, int toxMult, float step, float speed) GetStep()
    {
        return (Random.Range(rangeCost.x, rangeCost.y) * (_step + 1),
            Random.Range(rangeTox.x, rangeTox.y) * (_step + 1), Random.Range(size.x, size.y),
            Random.Range(speedRange.x, speedRange.y) * (_step + 1));
    }
}