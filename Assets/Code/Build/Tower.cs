using System;
using System.Collections.Generic;
using Code.InputSearch;
using Code.Saves;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Build
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private DragBreak itemPrefab;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private Transform targetDrop;

        public Vector2 Size { get; private set; }

        public List<DragBreak> Items { get; } = new();
        public Vector3 Up { get; private set; }

        private Vector3 _lastPos;

        private void Start()
        {
            Size = itemPrefab.GetComponent<SpriteRenderer>().bounds.size;
        }

        public Transform Build(Up own) => Build(new TowerData()
            { point = _lastPos = own.point, color = own.transform.GetComponent<Image>().color });

        private Transform Build(TowerData own)
        {
            if (transform.childCount > 0)
                own.point = Up + new Vector3(Random.Range(-Size.x, Size.x) * 0.5f, Size.y);
            else
                own.point = own.point;

            return BuildSort(own);
        }

        private Transform BuildSort(TowerData own)
        {
            Up = own.point;

            var p = Instantiate(itemPrefab, _lastPos, Quaternion.identity, transform);
            Items.Add(p);

            var c = own.color;
            var s = p.GetComponent<SpriteRenderer>();
            s.color = c;
            p.color = c;

            FitBoxCollider2DToChildren();

            return p.transform;
        }

        private void FitBoxCollider2DToChildren()
        {
            boxCollider.enabled = false;

            return;

            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

            Bounds combinedBounds = spriteRenderers[0].bounds;

            for (int i = 1; i < spriteRenderers.Length; i++)
                combinedBounds.Encapsulate(spriteRenderers[i].bounds);
            Vector3 localCenter = transform.InverseTransformPoint(combinedBounds.center);
            Vector3 localSize = combinedBounds.size;

            boxCollider.offset = localCenter;
            boxCollider.size = localSize;
        }


        public float MoveDown(int index)
        {
            float blockSizeY = Items[index].GetComponent<SpriteRenderer>().bounds.size.y;

            Up -= Vector3.up * blockSizeY;

            Items.RemoveAt(index);

            if (Items.Count <= 0)
                boxCollider.enabled = true;

            return blockSizeY;
        }

        public void Load(TowerSave saveManagerSaveTower)
        {
            var i = saveManagerSaveTower.Items;
            if (i == null || i.Length <= 0)
                return;
            foreach (var item in i)
            {
                var p = BuildSort(item);
                p.transform.position = item.point;
            }
        }

        private SaveManager _saveManager = new();

        private void OnEnable()
        {
            _saveManager.LoadFromFile();
            Load(_saveManager.SaveTower);
        }

        private void OnDestroy()
        {
            _saveManager.SaveTower.Save(Items.ToArray());
            _saveManager.SaveToFile();
        }
    }
}