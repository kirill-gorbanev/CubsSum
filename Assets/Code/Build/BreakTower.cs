using System;
using Code.InputSearch;
using Code.Signal;
using DG.Tweening;
using UnityEngine;

namespace Code.Build
{
    [Serializable]
    public struct BreakTower : ISelector<Up>
    {
        [SerializeField] private Tower tower;
        [SerializeField] private float speed;

        public Type FiltType => typeof(DragBreak);

        public void Event(Up own)
        {
            if (!own.transform.TryGetComponent(out DragBreak dragBreak))
                return;

            int index = tower.Items.IndexOf(dragBreak);
            if (index == -1) return;

            var s = tower.MoveDown(index);


            var ip = tower.Items[index].transform.position.y - s;

            for (int j = index; j < tower.Items.Count; j++)
            {
                Transform blockTransform = tower.Items[j].transform;
                blockTransform.DOMoveY(ip, speed);
                ip += s;
            }
        }
    }
}