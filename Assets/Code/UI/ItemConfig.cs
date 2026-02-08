using System;
using System.Linq;
using Code.Grid.Form;
using UnityEngine;

namespace Code.UI
{
    [CreateAssetMenu]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] public Config[] forms;
        [SerializeField] public Vector3 size;

        [SerializeField] public ViewCell[] hasCell;

        public ViewCell[] GetCells(FormsType types)
        {
            var cells = new ViewCell[types.cells.Length];

            for (int i = 0; i < cells.Length; i++)
                cells[i] = hasCell.First(e => e.id == types.cells[i].id);

            return cells;
        }
        public ViewCell GetCells(int id)
        {
            return  hasCell.First(e => e.id == id);
        }
    }

    [Serializable]
    public struct FormsType
    {
        public TypeCell[] cells;
    }

    [Serializable]
    public struct Config
    {
        public FormConstruct form;
        public FormsType[] variablesType;
    }
}