using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;

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
        [SerializeField] private Toggle toggle;

        private int _last;
        private int _max;
        private int _step;
        private int _mult = 1;

        private void Start()
        {
            _step = 1;
            _max = _step * levelsUp;
            counterText.text = YG2.Save.count.ToString();
            slider.value = (YG2.Save.count - _last) / (float)_max;
            level.text = string.Format(formLevel, _step);

            btX2.onClick.AddListener(() =>
            {
                YG2.RewardedAdvShow("x2", () =>
                {
                    btX2.enabled = false;
                    StartCoroutine(Time());
                });
            });

            zoneController.OnChange += b =>
            {
                YG2.Save.count += _mult;
                counterText.text = YG2.Save.count.ToString();

                level.text = string.Format(formLevel, _step);
                if (YG2.Save.count > _max)
                {
                    _step++;
                    _last = _max;
                    _max = _step * levelsUp;
                }

                slider.value = (YG2.Save.count - _last) / (float)_max;
            };

            StartCoroutine(Ads());


            toggle.onValueChanged.AddListener(_ =>
            {
                YG2.SetLeaderboard("clicks", YG2.Save.count);
                YG2.GetLeaderboard("clicks");
            });
        }

        private IEnumerator Time()
        {
            _mult = 2;
            yield return new WaitForSeconds(60);
            _mult = 1;

            yield return new WaitForSeconds(2 * 60);
            btX2.enabled = true;
        }

        [SerializeField] private GameObject ads;

        private IEnumerator Ads()
        {
            var s = new WaitForSeconds(2 * 60);
            var ss = new WaitForSeconds(2);
            while (true)
            {
                yield return s;
                ads.SetActive(true);
                yield return ss;
                ads.SetActive(false);

                YG2.InterstitialAdvShow();
            }
        }

        public void ToLink()
        {
            Application.OpenURL("https://t.me/berd_games");
        }
    }
}