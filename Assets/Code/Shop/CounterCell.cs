using System.Linq;
using TMPro;
using UnityEngine;
using YG;

namespace Code.Shop
{
    public class CounterCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text textCount;
        [SerializeField] private string formater;
        [SerializeField] public int max;

        public int id;

        private void Start()
        {
            textCount.text = string.Format(formater, 0, max);
            if (YG2.saves.Peres != null)
            {
                var s = YG2.saves.Peres.FirstOrDefault(e => e.id == id);

                if (s != null)
                    textCount.text = string.Format(formater, s.count, max);
            }
        }

        public void View(int id)
        {
            var s = YG2.saves.Peres.FirstOrDefault(e => e.id == id);

            if (s != null)
                textCount.text = string.Format(formater, s.count, max);
        }

        public bool IsMax(int id) => YG2.saves.Peres?.FirstOrDefault(e => e.id == id)?.count >= max;
    }
}