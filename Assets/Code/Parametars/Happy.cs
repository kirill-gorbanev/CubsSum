using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Parametars
{
    public class Happy : MonoBehaviour
    {
        [SerializeField] public float valueInit;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Slider slider;
        [SerializeField] private UnityEvent died;

        public float _value;

        private void Start()
        {
            _value = valueInit;
            text.text = _value.ToString();
            slider.value = _value/valueInit;
        }

        public void Damage(float value)
        {
            _value -= value;
            slider.value = _value/valueInit;
            text.text = _value.ToString();
            if (_value <= 0) 
                died?.Invoke();
        }
    }
}