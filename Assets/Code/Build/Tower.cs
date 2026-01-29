using System.Collections.Generic;
using Code.InputSearch;
using UnityEngine;
using UnityEngine.Serialization;
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

        private void Start()
        {
            Size = itemPrefab.GetComponent<SpriteRenderer>().bounds.size;
        }

        public Transform Build(Up own)
        {
            if (transform.childCount > 0)
                Up += new Vector3(Random.Range(-Size.x, Size.x) * 0.5f, Size.y);
            else
                Up = own.point;

            var p = Instantiate(itemPrefab, own.point, Quaternion.identity, transform);
            Items.Add(p);

            var c = own.transform.GetComponent<Image>().color;
            var s = p.GetComponent<SpriteRenderer>();
            s.color = c;

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
    }
}