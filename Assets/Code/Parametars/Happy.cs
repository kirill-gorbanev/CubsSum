using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Parametars
{
    public class Happy : MonoBehaviour
    {
        [SerializeField] private float valueInit;
        [SerializeField] private TMP_Text text;
        [SerializeField] private UnityEvent died;

        private float _value;

        private void Start()
        {
            _value = valueInit;
            text.text = _value.ToString();
        }

        public void Damage(float value)
        {
            _value -= value;
            text.text = _value.ToString();
            if (_value <= 0) 
                died?.Invoke();
        }
    }
}