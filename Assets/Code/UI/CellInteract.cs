using System;
using System.Collections.Generic;
using Code.Build;
using Code.InputSearch;
using Code.Sector;
using Code.Sector.UI;
using Code.Signal;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.UI
{
    public class CellInteract : MonoBehaviour, IFilt, ISelector<Down>, ISelector<Up>, ISelector<Drag>
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image image;

        [Space] [SerializeField] private int fragmentsCount = 8;

        [Space] [SerializeField] private IsDragAndMouse dr;
        [SerializeField] private IsHit hi;
        [SerializeField] private IsTower tw;
        [SerializeField] private IsLimit lm;

        private readonly List<ISect> _sects = new();


        private Vector2 _offset;
        private Camera _camera;

        public bool IsDrag { get; private set; }
        public GameObject Hit { get; set; }
        public Tower Tower { get; set; }

        public Server<Up> ServerUp { get; } = new();
        public Server<Over> ServerOver { get; } = new();
        public GameObject Item => gameObject;

        public Type FiltType => typeof(Cell);

        public void Event(Down own)
        {
            gameObject.SetActive(true);
            _offset = (Vector2)own.transform.position - own.point;
            IsDrag = true;

            image.color = own.transform.GetComponent<Cell>().ImageColor;
        }

        public void Event(Drag own)
        {
            rectTransform.position = own.origin + _offset;
        }

        public void Event(Up own)
        {
            var s = Find();
            if (!s)
            {
                ServerOver.Execute(new Over { isLimit = false }, this);
                Explode();
            }
            else
                ServerUp.Execute(new Up { transform = transform, point = _camera.ScreenToWorldPoint(rectTransform.position) }, this);

            IsDrag = false;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _camera = Camera.main;

            _sects.Add(dr);
            _sects.Add(hi);
            _sects.Add(tw);
            _sects.Add(lm);
        }

        private bool Find()
        {
            foreach (var sect in _sects)
                if (!sect.Execute())
                    return false;

            return true;
        }

        private void Explode(float duration = 0.4f)
        {
            for (int i = 0; i < fragmentsCount; i++)
            {
                GameObject fragment = new GameObject($"Fragment_{i}");
                fragment.transform.SetParent(rectTransform.parent);
                fragment.transform.position = rectTransform.position;

                Image fragImage = fragment.AddComponent<Image>();
                fragImage.sprite = image.sprite;
                fragImage.color = image.color;
                fragImage.raycastTarget = false;

                float size = rectTransform.rect.width / 3f;
                RectTransform rt = fragment.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(size, size);

                float angle = i * (360f / fragmentsCount) * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Vector3 targetPos = rectTransform.position + (Vector3)(direction * Random.Range(80f, 150f));

                Sequence fragSeq = DOTween.Sequence();
                fragSeq.Append(rt.DOMove(targetPos, duration).SetEase(Ease.InQuad))
                    .Join(rt.DOScale(0f, duration))
                    .Join(fragImage.DOFade(0f, duration))
                    .OnComplete(() => Destroy(fragment));
            }
        }
    }

}