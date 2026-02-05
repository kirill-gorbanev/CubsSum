using Code.Grid;
using UnityEngine;

namespace Code.UI
{
    public class Items : MonoBehaviour
    {
        [SerializeField] private ItemConfig config;
        [SerializeField] private Transform parent;
        [SerializeField] public Prefab items;
        public Spawner spawner;

        private void Start()
        {
            foreach (var fr in config.forms)
            {
                var p = Instantiate(items, parent);
                var v = Instantiate(fr, p.transform);
               v.size= v.transform.localScale = config.size;
                v.transform.position += fr.offset;
                p.Content = v;
                v.spawner = spawner;
                
            }
        }
    }
}