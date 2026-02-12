using System;
using System.Collections.Generic;
using System.Linq;
using Code.Grid.Form;
using UnityEngine;

namespace Code.UI
{
    [CreateAssetMenu]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] public FormConstruct[] forms;
        [SerializeField] public TypeCell[] cells;
        [SerializeField] public Vector3 size;


        public FormsType[] variablesType;

        private Dictionary<TypeCell, List<Compatible>> _gridCont;

        public Dictionary<TypeCell, List<Compatible>> GridCont =>
            _gridCont ??= cells.ToDictionary(e => e, e => e.compatible);

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