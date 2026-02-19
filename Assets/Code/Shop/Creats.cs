using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Creats : MonoBehaviour
{
    [SerializeField] private Button cellPr;
    [SerializeField] private Transform parent;
    [SerializeField] private int countCell;

    [SerializeField] private int stepCost;
    [SerializeField] private int initCost;

    [SerializeField] private Vector2Int rangeCost;
    [SerializeField] private Vector2Int rangeTox;
    [SerializeField] private Vector2 size;

    public event Func<int, bool> OnBye;

    private int _step;

    private void Start()
    {
        for (int i = 0; i < countCell; i++)
        {
            var c = Instantiate(cellPr, parent);
            c.gameObject.SetActive(true);
            var cost = initCost + stepCost * i;
            c.GetComponentInChildren<TMP_Text>().text = cost.ToString();
            c.onClick.AddListener(() =>
            {
                if (OnBye.Invoke(cost))
                {
                    _step = i+1;
                    c.gameObject.SetActive(false);
                }
            });
        }
    }

    public (int coinMult, int toxMult, float step) GetStep()
    {
        return (Random.Range(rangeCost.x, rangeCost.y) * (_step + 1), Random.Range(rangeTox.x, rangeTox.y) * (_step+1),Random.Range(size.x, size.y));
    }
}