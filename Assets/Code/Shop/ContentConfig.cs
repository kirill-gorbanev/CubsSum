using System;
using UnityEngine;

namespace Code.Shop
{
    [CreateAssetMenu]
    public class ContentConfig : ScriptableObject
    {
        [SerializeField] private GameObject[] view;
        [SerializeField] private int costInit;
        [SerializeField] private bool isDetect;
        
        public Item[] items;

        private void OnValidate()
        {
            if(isDetect)
                return;
            items = new Item[view.Length];
            for (int i = 0; i < view.Length; i++)
                items[i] = new Item{view = view[i] , cost = costInit * (i + 1)};
        }
    }

    [Serializable]
    public struct Item
    {
        public GameObject view;
        public string descr;
        public int cost;
    }
}