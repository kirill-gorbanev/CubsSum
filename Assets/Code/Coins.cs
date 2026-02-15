using UnityEngine;

namespace Code
{
    public class Coins : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;

        public int coin;

        private void Awake()
        {
            zone.OnChange += e => coin += e ? 1 : -1;
        }
    }
}