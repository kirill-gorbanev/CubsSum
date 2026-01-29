using System;
using Code.InputSearch;
using Code.Signal;
using Code.UI;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Code.Build
{
   
    [Serializable]
    public struct BuildTower : ISelector<Up>
    {
        [SerializeField] private Tower tower;
        [SerializeField] private float speed;

        public Type FiltType => typeof(CellInteract);

        public void Event(Up own)
        {
            var p = tower.Build(own);

            if (tower.Items.Count > 1)
                DoAnimSetTower(p, tower.Up);
            else
                p.position = tower.Up;
        }

        private void DoAnimSetTower(Transform p, Vector3 u)
        {
            float offset = 1f;
            Vector3 aboveTarget = u + Vector3.up * offset;

            Sequence seq = DOTween.Sequence();

            seq.Append(p.DOMove(aboveTarget, speed).SetEase(Ease.OutQuad));

            seq.Append(p.DOMove(u, speed * 0.5f).SetEase(Ease.InQuad));
        }
    }

}