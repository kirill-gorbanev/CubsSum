using UnityEngine;
using YG;

namespace Code
{
    public class Coins : MonoBehaviour
    {
        public int mult = 1;

        public void Add()
        {
            YG2.saves.coins += mult;
            YG2.SaveProgress();
        }
    }
}