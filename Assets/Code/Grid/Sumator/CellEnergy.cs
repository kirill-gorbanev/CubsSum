using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Grid.Sumator
{
    public class CellEnergy : MonoBehaviour
    {
        [SerializeField] private Button add;
        [SerializeField] private Button min;
        [SerializeField] public TMP_Text text;
        
        public event Action<bool> OnChange;

        private void Start()
        {
            add.onClick.AddListener(() => OnChange?.Invoke(true));
            min.onClick.AddListener(() => OnChange?.Invoke(false));
        }
    }
}