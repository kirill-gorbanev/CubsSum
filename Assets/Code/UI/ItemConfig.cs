using System;
using System.Linq;
using Code.Grid.Form;
using UnityEngine;

namespace Code.UI
{
    [CreateAssetMenu]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] public FormConstruct[] forms;
        [SerializeField] public Vector3 size;

        [SerializeField] public ViewCell[] hasCell;

        public FormsType[] variablesType;

        public ViewCell[] GetCells(FormsType types)
        {
            var cells = new ViewCell[types.cells.Length];

            for (int i = 0; i < cells.Length; i++)
                cells[i] = hasCell.First(e => e.id == types.cells[i].id);

            return cells;
        }

        public ViewCell GetCells(int id)
        {
            return hasCell.First(e => e.id == id);
        }

        public FormsType GetForm(int countCells)
        {
            var s = variablesType.Where(e => e.cells.Length == countCells).ToList();
            return s[UnityEngine.Random.Range(0, s.Count)];
        }
    }

    [Serializable]
    public struct FormsType
    {
        public TypeCell[] cells;
        public bool isGroup;
    }
}