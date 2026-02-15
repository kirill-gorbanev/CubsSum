using TMPro;
using UnityEngine;

namespace Code
{
    public class DetectCounter : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;
        [SerializeField] private TMP_Text counter;

        [SerializeField] private Coins count;

        private void Start()
        {
            counter.text = count.coin.ToString();
            
            zone.OnChange += _ => counter.text = count.coin.ToString();
        }
    }
}