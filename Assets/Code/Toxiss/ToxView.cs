using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Toxiss
{
    public class ToxView : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Toxis toxis;
        [SerializeField] private Button btDetox;
        [SerializeField] private TMP_Text text;
        [SerializeField] private TMP_Text textMult;

        public UnityAction OnBtClicked { get; set; }

        private void Start()
        {
            btDetox.onClick.AddListener(OnBtClicked);
            View();
        }

        public void View()
        {
            slider.value = (float)toxis.toxis / toxis.maxTox;
        }
        
        public void View(string value, string mult)
        {
            text.text = value;
            textMult.text = mult;
        }
    }
}