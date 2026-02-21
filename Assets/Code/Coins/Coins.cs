using UnityEngine;

namespace Code
{
    public class Coins : MonoBehaviour
    {
        public int mult = 1;
        public double coin;

        public void Add()
        {
            coin += mult;
        }
    }
}