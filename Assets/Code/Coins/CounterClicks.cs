using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Code
{
    public class CounterClicks : MonoBehaviour
    {
        [SerializeField] private ZoneController zoneController;
        [SerializeField] private TMP_Text counterText;
        [SerializeField] private TMP_Text level;
        [SerializeField] private string formLevel;
        [SerializeField] private Slider slider;
        [SerializeField] private int levelsUp;
        [SerializeField] private Button btX2;

        private int _count;
        private int _last;
        private int _max;
        private int _step;
        private int _mult = 1;

        private void Start()
        {
            _step = 1;
            _max = _step * levelsUp;
            counterText.text = _count.ToString();
            slider.value = (_count - _last) / (float)_max;
            level.text = string.Format(formLevel, _step);

            btX2.onClick.AddListener(() => { YG2.RewardedAdvShow("x2", () => { StartCoroutine(Time()); }); });

            zoneController.OnChange += b =>
            {
                _count += _mult;
                counterText.text = _count.ToString();

                level.text = string.Format(formLevel, _step);
                if (_count > _max)
                {
                    _step++;
                    _last = _max;
                    _max = _step * levelsUp;
                }

                slider.value = (_count - _last) / (float)_max;
            };
        }

        private IEnumerator Time()
        {
            _mult = 2;

            yield return new WaitForSeconds(60);

            _mult = 1;
        }

        public void ToLink()
        {
            Application.OpenURL("https://t.me/berd_games");
        }
    }
}