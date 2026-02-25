using TMPro;
using UnityEngine;

namespace Code.Shop
{
    public class CounterCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text textCount;
        [SerializeField] private string formater;
        [SerializeField] public int max;

        private int count = 0;

        private void Start()
        {
            textCount.text = string.Format(formater, count, max);
            count = 1;
        }

        public void Add()
        {
            count++;
            textCount.text = string.Format(formater, count, max);
        }
        public bool IsMax => count >= max;
    }
}