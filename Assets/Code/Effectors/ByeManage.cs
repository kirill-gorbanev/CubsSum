using System;
using UnityEngine;

namespace Code.Effectors
{
    [Serializable]
    public class ByeManage 
    {
        [SerializeField] private int cost;
        [SerializeField] private int velocityLiner;
        [SerializeField] private Coins coins;

        private int _step;

        public event Action<int> OnByeCompleted; 
        public int Cost => cost + velocityLiner * _step;

        public void Bye()
        {
            if (coins.coin < Cost)
                return;
            coins.coin -= Cost;
            _step++;
            OnByeCompleted?.Invoke(_step);
        }
    }
}