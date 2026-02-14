using System.Linq;
using Code.UI;
using TMPro;
using UnityEngine;

namespace Code.Grid.Sumator
{
    public class UseEnergy : MonoBehaviour
    {
        [SerializeField] private ItemConfig currentItem;
        [SerializeField] private TypeRes energyType;
        [SerializeField] private Spawner spawner;
        
        [SerializeField] private Transform parent;
        [SerializeField] private TMP_Text energyText;
        
        private TypeCell[] _typesFind;

        private void Start()
        {
            _typesFind = currentItem.cells.Where(e => e.use.Contains(energyType)).ToArray();
            
            spawner.OnLoad += FindEnergy;
        }

        private void FindEnergy()
        {
            foreach (var cell in _typesFind)
            {
               var t= Instantiate(energyText, parent);
               t.gameObject.SetActive(true);
               t.text = cell.info;
            }
        }
    }
}