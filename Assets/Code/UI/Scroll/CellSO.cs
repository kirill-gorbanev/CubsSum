using DefaultNamespace;
using UnityEngine;

namespace Code.UI.Scroll
{
    [CreateAssetMenu]
    public class CellSO : ScriptableObject
    {
        [SerializeField, Range(0, 1), Tooltip("насыщенность")] private float s = 1;

        [SerializeField, Range(0, 1), Tooltip("яркость")] private float v = 1;

        [SerializeField] private Cell cellPrefab;
        [SerializeField] private int count;

        public int Count => count;
        public Cell Prefab => cellPrefab;

        public Color GetColor(float hue) => Color.HSVToRGB(hue, s, v);
    }
}