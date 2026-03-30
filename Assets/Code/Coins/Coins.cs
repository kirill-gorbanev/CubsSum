using UnityEngine;
using YG;

namespace Code
{
    public class Coins : MonoBehaviour
    {
        public int mult = 1;
        public CounterClicks Clicks;

        public void Add()
        {
            YandexGame.savesData.coins += mult * Clicks._mult;
        }
    }
}