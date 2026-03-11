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
        [SerializeField] private TMP_Text multTx;
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

            if (YG2.saves.step <= 0)
                YG2.saves.step = 1;

            _max = YG2.saves.step * levelsUp;
            counterText.text = YG2.saves.count.ToString();
            slider.value = (YG2.saves.count - YG2.saves.last) / ((float)_max - YG2.saves.last);
            level.text = string.Format(formLevel, YG2.saves.step);
            multTx.text = "x" + _mult;

            btX2.onClick.AddListener(() =>
            {
                foreach (var a in activesAds)
                    a.SetActive(false);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(false);
                AudioListener.volume = 0f;

                YG2.RewardedAdvShow("x2", () => { StartCoroutine(Time()); });
            });

            zoneController.OnChange += b =>
            {
                YG2.saves.count += _mult;
                counterText.text = YG2.saves.count.ToString();

                level.text = string.Format(formLevel, YG2.saves.step);
                if (YG2.saves.count > _max)
                {
                    YG2.saves.step++;
                    YG2.saves.last = _max;
                    _max = YG2.saves.step * levelsUp;
                    YG2.SetLeaderboard("clicks", YG2.saves.step);
                }

                slider.value = (YG2.saves.count - YG2.saves.last) / ((float)_max - YG2.saves.last);
                YG2.SaveProgress();
            };

            StartCoroutine(Ads());


            toggle.onValueChanged.AddListener(_ =>
            {
                YG2.SetLeaderboard("clicks", YG2.saves.count);
                YG2.GetLeaderboard("clicks");
            });

            YG2.onCloseAnyAdv += () =>
            {
                ads.SetActive(false);
                foreach (var a in activesAds)
                    a.SetActive(true);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(true);
                AudioListener.volume = YG2.saves.isSounds ? 1f : 0f;
            };
        }

        private IEnumerator Time()
        {
            btX2.gameObject.SetActive(false);

            _mult = 2;
            multTx.text = "x" + _mult;

            yield return new WaitForSeconds(60);
            _mult = 1;
            multTx.text = "x" + _mult;

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