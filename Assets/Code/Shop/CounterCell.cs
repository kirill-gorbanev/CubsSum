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
            if(YG2.saves.Pers.Count > id )
            textCount.text = string.Format(formater, YG2.saves.Pers[id].count, max);
        }

        public void View()
        {
            textCount.text = string.Format(formater, YG2.saves.Pers[id].count, max);
        }

        public bool IsMax => YG2.saves.Pers.Count > id && YG2.saves.Pers[id].count >= max;
    }
}