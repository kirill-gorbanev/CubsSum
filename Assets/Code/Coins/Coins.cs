using UnityEngine;

namespace Code
{
    public class Coins : MonoBehaviour
    {
        public int mult = 1;
        public int coin;

        public void Add()
        {
            coin += mult;
        }
    }
}