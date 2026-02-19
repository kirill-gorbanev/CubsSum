using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class Creats 
{
    [SerializeField] private Button cellPr;
    [SerializeField] private Transform parent;
    [SerializeField] private int countCell;

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
        for (int i = 0; i < countCell; i++)
        {
            var c = Object. Instantiate(cellPr, parent);
            c.gameObject.SetActive(true);
            var cost = initCost + stepCost * i;
            c.GetComponentInChildren<TMP_Text>().text = cost.ToString();
            var i1 = i;
            c.onClick.AddListener(() =>
            {
                if (OnBye.Invoke(cost))
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