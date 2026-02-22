using System;
using TMPro;
using UnityEngine;

namespace Code.Shop
{
    public class CounterCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text textCount;
        [SerializeField] private string formater;
        [SerializeField] private int max;

        private int count;

        private void Start()
        {
            textCount.text = string.Format(formater, count, max);

        }

        public void Add()
        {
            count++;
            textCount.text = string.Format(formater, count, max);
        }
        public bool IsMax => count >= max;
    }
}