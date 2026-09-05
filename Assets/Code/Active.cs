using System.Collections;
using Audio;
using Code.Pers;
using Code.Shop.Donat;
using UnityEngine;
using YG;
using YG.Insides;
using Random = UnityEngine.Random;

namespace Code
{
    public class Active : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;
        [SerializeField] private Coins coins;

        [SerializeField] private CoinView coinsView;
        [SerializeField] private Moves moves;

        [SerializeField] public Creats creatsZone;
        [SerializeField] public Creats creatsPassive;
        [SerializeField] public Audios audioZone;

        [SerializeField] public GameObject[] viewsSmokes;

        [SerializeField] private Sigar sigar;
        [SerializeField] private PersCell pers;
        [SerializeField] private AddPersCounter add;

        private void Awake()
        {
            YandexGame.GetDataEvent += () =>
            {
                zone.OnChange += e =>
                {
                    if (e)
                    {
                        coins.Add();
                        coinsView.View();
                    }
                };

                creatsZone.OnBye += cost =>
                {
                    if (YandexGame.savesData.coins < cost)
                        return false;

                    YandexGame.savesData.coins -= cost;
                    audioZone.Bye();
                    return true;
                };
                creatsZone.OnComplete += () =>
                {
                    var v = creatsZone.GetStep();
                    coins.mult = v.coinMult;
                    zone.speed = v.speed;
                    zone.Reload(v.size);

                    coinsView.View();

                    foreach (var vv in viewsSmokes)
                        vv.SetActive(false);
                    viewsSmokes[creatsZone._step].SetActive(true);
                };
                creatsZone.Start();


                creatsPassive.OnBye += cost =>
                {
                    if (YandexGame.savesData.coins < cost)
                        return false;
                    YandexGame.savesData.coins -= cost;
                    audioZone.Bye();
                    return true;
                };
                creatsPassive.OnComplete += () =>
                {
                    coinsView.View();
                    var pers = Instantiate(creatsPassive.view).GetComponent<Pers.Pers>();
                    moves.pres.Add(new Moves.MyStruct
                    {
                        pers = pers,
                        add = Random.Range(creatsPassive._rangeCost.x, creatsPassive._rangeCost.y),
                    });
                    moves.Ranger(pers.transform);
                };
                moves.OnAdd += e =>
                {
                    YandexGame.savesData.coins += e;
                    coinsView.View();
                    coinsView.passive.text = string.Format(coinsView.formatPassive, e);
                };

                creatsPassive.Start();
            };

            savess();

            YandexGame.onShowWindowGame += YandexGame.SaveProgress;
            YandexGame.onHideWindowGame += YandexGame.SaveProgress;
            YandexGame.PurchaseSuccessEvent += _ => YandexGame.SaveProgress();

            StartCoroutine(TimeSave());
        }

        private void OnDestroy()
        {
            YandexGame.SaveProgress();
        }

        private void OnDisable()
        {
            YandexGame.SaveProgress();
        }

        private IEnumerator TimeSave()
        {
            var t = new WaitForSeconds(1f);
            while (true)
            {
                yield return t;
                YandexGame.SaveProgress();
            }
        }

        private void savess()
        {
            YandexGame.GetDataEvent  += () =>
            {
                YandexGame.savesData.Pods ??= new();
                YandexGame.savesData.Peres ??= new();
                
                coinsView.View();

                foreach (var v in viewsSmokes)
                    v.SetActive(false);
                viewsSmokes[YandexGame.savesData.activeIdPods].SetActive(true);

                sigar.Load();
                pers.Load();
                add.Load();
            };

            YandexGame.LoadProgress();
        }
    }
}