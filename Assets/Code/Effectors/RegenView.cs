using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Effectors
{
    public class RegenView : MonoBehaviour
    {
        [SerializeField] private Button bt;
        [SerializeField] private TMP_Text text;

        public UnityAction OnBtClicked { get; set; }

        private void Start()
        {
            bt.onClick.AddListener(OnBtClicked);
        }

        public void View(string value)
        {
            text.text = value;
        }
    }
}