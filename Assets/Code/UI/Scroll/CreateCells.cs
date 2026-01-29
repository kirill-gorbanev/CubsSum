using Code.InputSearch;
using Code.Signal;
using UnityEngine;

namespace Code.UI.Scroll
{
    public class CreateCells : MonoBehaviour
    {
        [SerializeField] private CellSO config;
        [SerializeField] private Transform parent;
        [SerializeField] private CellInteract cellInteract;

        public Server<Down> ServerDown { get; } = new();
        public Server<Drag> ServerDrag { get; } = new();
        public Server<Up> ServerUp { get; } = new();

        private void Start()
        {
            var m = config.Count;
            for (int i = 0; i < m; i++)
            {
                var cell = Instantiate(config.Prefab, parent);
                cell.Init(config.GetColor((float)i / m), this);
            }
        }
    }
}