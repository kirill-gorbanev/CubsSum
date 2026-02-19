using TMPro;
using UnityEngine;

namespace Code
{
    public class CoinView : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;
        [SerializeField] private TMP_Text counter;
        [SerializeField] private TMP_Text mult;

        [SerializeField] private Coins count;

        private void Start()
        {
            View();
        }

        public void View()
        {
            counter.text = count.coin.ToString();
            mult.text = count.mult.ToString();
        }
    }
}