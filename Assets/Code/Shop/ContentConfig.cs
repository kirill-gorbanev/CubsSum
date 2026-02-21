using System;
using UnityEngine;

namespace Code.Shop
{
    public enum Range
    {
        Agress,
        Norm,
        Slay
    }
    
    [CreateAssetMenu]
    public class ContentConfig : ScriptableObject
    {
        [SerializeField] private GameObject[] view;
        [SerializeField] private bool isDetect;
        [SerializeField] public int costInit;
        [SerializeField] public Vector2Int rangeInitDamage;
        [SerializeField] public int dopDamage;
        [SerializeField] public Vector2Int rangeInitAdd;
        [SerializeField] public int dopAdd;

        [Space] [SerializeField] private float multSizeView;
        [SerializeField] private Vector3 offsetPos;
        [SerializeField] private Vector3 rot;

        public Item[] items;

        private void OnValidate()
        {
            if (isDetect)
                return;
            items = new Item[view.Length];
            for (int i = 0; i < view.Length; i++)
                if (!items[i].isDetect)
                    items[i] = new Item
                    {
                        view = view[i],
                        multSizeView = multSizeView,
                        offsetPos = offsetPos,
                        rot = rot,
                    };
        }
    }

    [Serializable]
    public struct Item
    {
        public bool isDetect;
        public GameObject view;

        public float multSizeView;
        public Vector3 offsetPos;
        public Vector3 rot;
        public Range range;
    }
}