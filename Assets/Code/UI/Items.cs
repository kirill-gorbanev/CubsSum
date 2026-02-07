using Code.Grid;
using Code.Grid.Form;
using UnityEngine;

namespace Code.UI
{
    public class Items : MonoBehaviour
    {
        [SerializeField] private ItemConfig config;

        [SerializeField] public Prefab current;
        [SerializeField] public Transform next;

        public Spawner spawner;

        private int _id;

        private void Start()
        {
            Next();

            current.OnDropItem += Next;
        }

        private void Next()
        {
            if (_id >= config.forms.Length)
                return;

            Load(config.forms[_id], current);
            Del(next);
            if (_id + 1 < config.forms.Length)
            {
                Load(config.forms[_id + 1], next);
            }

            _id++;
        }

        private void Del(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
                Destroy(parent.GetChild(i).gameObject);
        }

        private void Load(FormConstruct fr, Prefab p)
        {
            p.Content = Load(fr, p.transform);
        }

        private FormConstruct Load(FormConstruct fr, Transform p)
        {
            var v = Instantiate(fr, p.transform);
            v.size = v.transform.localScale = config.size;
            v.transform.position += fr.offset;
            v.spawner = spawner;

            return v;
        }
    }
}