using System.Collections;
using Code.Pers;
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
        [SerializeField] private Toggle toggle;

        private int _max;
        private int _mult = 1;

        private void Start()
        {
            YG2.InterstitialAdvShow();
            
            if (YG2.Save.step <= 0)
                YG2.Save.step = 1;
            
            _max = YG2.Save.step * levelsUp;
            counterText.text = YG2.Save.count.ToString();
            slider.value = (YG2.Save.count - YG2.Save.last) / ((float)_max - YG2.Save.last);
            level.text = string.Format(formLevel, YG2.Save.step);

            btX2.onClick.AddListener(() =>
            {
                foreach (var a in activesAds)
                    a.SetActive(false);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(false);
                AudioListener.volume = 0f;

                YG2.RewardedAdvShow("x2", () =>
                {
                    btX2.gameObject.SetActive(false);
                    StartCoroutine(Time());
                });
            });

            zoneController.OnChange += b =>
            {
                YG2.Save.count += _mult;
                counterText.text = YG2.Save.count.ToString();

                level.text = string.Format(formLevel, YG2.Save.step);
                if (YG2.Save.count > _max)
                {
                    YG2.Save.step++;
                    YG2.Save.last = _max;
                    _max = YG2.Save.step * levelsUp;
                    YG2.SetLeaderboard("clicks", YG2.Save.count);
                }

                slider.value = (YG2.Save.count - YG2.Save.last) / ((float)_max - YG2.Save.last);
            };

            StartCoroutine(Ads());


            toggle.onValueChanged.AddListener(_ =>
            {
                YG2.SetLeaderboard("clicks", YG2.Save.count);
                YG2.GetLeaderboard("clicks");
            });

            YG2.onCloseAnyAdv += () =>
            {
                ads.SetActive(false);
                foreach (var a in activesAds)
                    a.SetActive(true);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(true);
                AudioListener.volume = YG2.Save.isSounds ? 1f : 0f;
            };
        }

        private IEnumerator Time()
        {
            _mult = 2;
            yield return new WaitForSeconds(60);
            _mult = 1;

            yield return new WaitForSeconds(2 * 60);
            btX2.gameObject.SetActive(true);
        }

        [SerializeField] private GameObject ads;
        [SerializeField] private GameObject[] activesAds;
        [SerializeField] private Moves _moves;

        private IEnumerator Ads()
        {
            var s = new WaitForSeconds(2 * 60);
            var ss = new WaitForSeconds(2);
            while (true)
            {
                yield return s;

                ads.SetActive(true);
                foreach (var a in activesAds)
                    a.SetActive(false);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(false);
                AudioListener.volume = 0f;

                yield return ss;
                YG2.InterstitialAdvShow();
            }
        }

        public void ToLink()
        {
            Application.OpenURL("https://t.me/berd_games");
        }
    }
}