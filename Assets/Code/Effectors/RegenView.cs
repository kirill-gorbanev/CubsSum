using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Effectors
{
    public class RegenView : MonoBehaviour
    {
        [SerializeField] private Regen regen;
        [SerializeField] private Button bt;
        [SerializeField] private TMP_Text text;

        public UnityAction OnBtClicked { get; set; }

        private void Start()
        {
            bt.onClick.AddListener(OnBtClicked);
            View();
        }

        public void View()
        {
            text.text = regen.Cost.ToString();
        }
    }
}