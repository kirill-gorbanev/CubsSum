using System.Linq;
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

            var ss = config.forms[_id].variablesType;
            Load(config.forms[_id].form, ss[Random.Range(0, ss.Length - 1)], current);
            
            Del(next);
            if (_id + 1 < config.forms.Length)
            {
                var s = config.forms[_id + 1].variablesType;
                Load(config.forms[_id + 1].form, s[Random.Range(0, s.Length - 1)], next);
            }

            _id++;
        }

        private void Del(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
                Destroy(parent.GetChild(i).gameObject);
        }

        private void Load(FormConstruct fr, FormsType formsType, Prefab p)
        {
            p.Content = Load(fr, formsType, p.transform);
        }

        private FormConstruct Load(FormConstruct fr, FormsType formsType, Transform p)
        {
            var v = Instantiate(fr, p.transform);
            v.size = v.transform.localScale = config.size;
            v.transform.position += fr.offset;
            v.spawner = spawner;
            
            v.LoadView(config.GetCells(formsType));
            
            return v;
        }
    }
}