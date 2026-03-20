using UnityEngine;
using YG;

namespace Code
{
    public class Coins : MonoBehaviour
    {
        public int mult = 1;

        public void Add()
        {
            YandexGame.savesData.coins += mult;
        }
    }
}