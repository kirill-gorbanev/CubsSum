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
            YandexGame.FullscreenShow();

            if (YandexGame.savesData.step <= 0)
                YandexGame.savesData.step = 1;

            _max = YandexGame.savesData.step * levelsUp;
            counterText.text = YandexGame.savesData.count.ToString();
            slider.value = (YandexGame.savesData.count - YandexGame.savesData.last) / ((float)_max - YandexGame.savesData.last);
            level.text = string.Format(formLevel, YandexGame.savesData.step);
            multTx.text = "x" + _mult;

            btX2.onClick.AddListener(() =>
            {
                foreach (var a in activesAds)
                    a.SetActive(false);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(false);
                AudioListener.volume = 0f;

                YandexGame.RewVideoShow(1);
                YandexGame.RewardVideoEvent +=  (e) => {if(e==1) StartCoroutine(Time()); };
            });

            zoneController.OnChange += b =>
            {
                YandexGame.savesData.count += _mult;
                counterText.text = YandexGame.savesData.count.ToString();

                level.text = string.Format(formLevel, YandexGame.savesData.step);
                if (YandexGame.savesData.count > _max)
                {
                    YandexGame.savesData.step++;
                    YandexGame.savesData.last = _max;
                    _max = YandexGame.savesData.step * levelsUp;
                    YandexGame.NewLeaderboardScores("clicks", YandexGame.savesData.step);
                }

                slider.value = (YandexGame.savesData.count - YandexGame.savesData.last) / ((float)_max - YandexGame.savesData.last);
            };

            StartCoroutine(Ads());


            toggle.onValueChanged.AddListener(_ =>
            {
                YandexGame.NewLeaderboardScores("clicks", YandexGame.savesData.count);
                YandexGame.GetLeaderboard("clicks",
                maxQuantityPlayers:10,
                    quantityTop:3,
                quantityAround:10,
                    photoSizeLB:"32");
            });

            YandexGame.CloseFullAdEvent += () =>
            {
                ads.SetActive(false);
                foreach (var a in activesAds)
                    a.SetActive(true);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(true);
                AudioListener.volume = YandexGame.savesData.isSounds ? 1f : 0f;
            };
            YandexGame.CloseVideoEvent += () =>
            {
                ads.SetActive(false);
                foreach (var a in activesAds)
                    a.SetActive(true);
                foreach (var a in _moves.pres)
                    a.pers.gameObject.SetActive(true);
                AudioListener.volume = YandexGame.savesData.isSounds ? 1f : 0f;
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
                YandexGame.FullscreenShow();
            }
        }

        public void ToLink()
        {
            Application.OpenURL("https://t.me/berd_games");
        }
    }
}