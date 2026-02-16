using UnityEngine;

namespace Code
{
    public class Coins : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;

        public int mult = 1;
        public int coin;

        private void Awake()
        {
            zone.OnChange += _ => coin += mult;
        }
    }
}