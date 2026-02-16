using System;
using UnityEngine;

namespace Code.Effectors
{
    public class Regen : MonoBehaviour
    {
        [SerializeField] private int cost;
        [SerializeField] private int velocityLiner;
        [SerializeField] private Coins coins;

        private int _step;

        public event Action OnRegen; 
        public int Cost => cost + velocityLiner * _step;

        public void Bye()
        {
            if (coins.coin < Cost)
                return;
            coins.coin -= Cost;
            _step++;
            OnRegen?.Invoke();
        }
    }
}