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
            if (YG2.Save.Peres != null && YG2.Save.Peres.Count > id)
                textCount.text = string.Format(formater, YG2.Save.Peres[id].count, max);
        }

        public void View()
        {
            if (YG2.Save.Peres != null && YG2.Save.Peres.Count > id)
            textCount.text = string.Format(formater, YG2.Save.Peres[id].count, max);
        }

        public bool IsMax => YG2.Save.Peres!= null&& YG2.Save.Peres.Count > id && YG2.Save.Peres[id].count >= max;
    }
}