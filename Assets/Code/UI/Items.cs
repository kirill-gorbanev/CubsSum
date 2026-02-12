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
        private FormsType _last;
        private FormConstruct _lastForm;

        private void Start()
        {
            _lastForm = config.forms[_id];
            _last = config.GetForm(_lastForm.Count);

            Next();

            current.OnDropItem += Next;
        }

        private void Next()
        {
            if (_id >= config.forms.Length)
                return;

            Load(_lastForm, _last, current);
            Del(next);

            if (_id + 1 < config.forms.Length)
            {
                _lastForm = config.forms[_id + 1];
                _last = config.GetForm(_lastForm.Count);

                Load(_lastForm, _last, next);
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
            v.IsGroup = formsType.isGroup;


            var c = config.variablesType.Where(e=>e.cells.Length == fr.grid.Length).ToArray();
            v.LoadView(c[Random.Range(0, c.Length)].cells);

            return v;
        }
    }
}