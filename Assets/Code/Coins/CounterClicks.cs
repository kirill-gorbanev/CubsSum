using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class CounterClicks : MonoBehaviour
    {
        [SerializeField] private ZoneController zoneController;
        [SerializeField] private TMP_Text counterText;
        [SerializeField] private Slider slider;
        [SerializeField] private int levelsUp;

        private int _count;
        private int _max;
        private int _step;

        private void Start()
        {
            _step = 1;
            _max = _step * levelsUp;
            counterText.text = _count.ToString();
            slider.value = _count / (float)_max;

            zoneController.OnChange += b =>
            {
                _count++;
                counterText.text = _count.ToString();
                if (_count > _max)
                {
                    _step++;
                    _max = _step * levelsUp;
                }

                slider.value = _count / (float)_max;
            };
        }
    }
}