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
        [SerializeField] public Button find;
        [SerializeField] public TMP_Text text;
        [SerializeField] public Image bg;
        
        public Action<bool> OnChange;

        private void Start()
        {
            add.onClick.AddListener(() => OnChange?.Invoke(true));
            min.onClick.AddListener(() => OnChange?.Invoke(false));
        }
    }
}