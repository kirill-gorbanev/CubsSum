using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Parametars
{
    public class Ecologi: MonoBehaviour
    {
        [SerializeField] private float valueInit;
        [SerializeField] private TMP_Text text;
        [SerializeField] private UnityEvent<float> add;

        private float _value;

        private void Start()
        {
            _value = valueInit;
            text.text = _value.ToString();
        }

        public void Add(float value)
        {
            _value += value;
            text.text = _value.ToString();
            add?.Invoke(_value);
        }
    }
}