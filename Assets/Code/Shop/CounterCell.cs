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
            if (YG2.Save.Peres != null)
            {
                var s = YG2.Save.Peres.FirstOrDefault(e => e.id == id);

                if (s != null)
                    textCount.text = string.Format(formater, s.count, max);
            }
        }

        public void View()
        {
            if (YG2.Save.Peres != null)
            {
                var s = YG2.Save.Peres.FirstOrDefault(e => e.id == id);

                if (s != null)
                    textCount.text = string.Format(formater, s.count, max);
            }
        }

        public bool IsMax
        {
            get
            {
                if (YG2.Save.Peres != null)
                {
                    var s = YG2.Save.Peres.FirstOrDefault(e => e.id == id);

                    if (s != null)
                        return s.count >= max;
                }

                return false;
            }
        }
    }
}