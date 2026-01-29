using System;
using Code.InputSearch;
using Code.Signal;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Build
{
   
    [Serializable]
    public class SelectBreak : ISelector<Up>, ISelector<Drag>
    {
        [SerializeField] private float height;
        [SerializeField] private float duration;
        
        [SerializeField] private Transform targetDrop;

        public void Event(Drag own)
        {
            own.item.transform.position = own.origin;
        }
        
        public Type FiltType => typeof(DragBreak);

        public void Event(Up own)
        {
            var tr = own.transform;
            tr.parent = null;

            Vector3 start = tr.position;
            
            Vector3 controlPoint = Vector3.Lerp(start,targetDrop.position, 0.5f);
            controlPoint += Vector3.up * height;
            
            float elapsed = 0f;
            tr.DOMoveX(0, duration).From(0).SetAutoKill(false)
                .OnUpdate(() =>
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / duration;
                    tr.position = QuadraticBezier(start, controlPoint, targetDrop.position, t);
                })
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Object.Destroy(tr.gameObject);
                });
        }
        
        private Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            return uu * p0 + 2 * u * t * p1 + tt * p2;
        }
    }
}