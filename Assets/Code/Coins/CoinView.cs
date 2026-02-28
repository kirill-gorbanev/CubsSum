using TMPro;
using UnityEngine;
using YG;

namespace Code
{
    public class CoinView : MonoBehaviour
    {
        [SerializeField] private TMP_Text counter;
        [SerializeField] private TMP_Text mult;
        [SerializeField] public TMP_Text passive;
        [SerializeField] public string formatPassive;

        [SerializeField] private Coins count;

        private void Start()
        {
            View();
        }

        public void View()
        {
            counter.text =NumberFormatter.Format( YG2.Save.coins);
            mult.text = count.mult.ToString();
        }
    }
}