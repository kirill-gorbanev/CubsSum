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
            YG2.onGetSDKData += () =>
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
                    if (YG2.saves.coins < cost)
                        return false;

                    YG2.saves.coins -= cost;
                    YG2.SaveProgress();
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
                    if (YG2.saves.coins < cost)
                        return false;
                    YG2.saves.coins -= cost;
                    audioZone.Bye();
                    YG2.SaveProgress();
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
                    YG2.saves.coins += e;
                    YG2.SaveProgress();
                    coinsView.View();
                    coinsView.passive.text = string.Format(coinsView.formatPassive, e);
                };

                creatsPassive.Start();
            };

            savess();

            YG2.onShowWindowGame += YG2.SaveProgress;
            YG2.onHideWindowGame += YG2.SaveProgress;
            YG2.onHideWindowGame += YG2.SaveProgress;
            YG2.onPauseGame += _ => YG2.SaveProgress();
            YG2.onFocusWindowGame += (_) => { YG2.SaveProgress(); };
        }

        private void OnDestroy()
        {
            YG2.SaveProgress();
        }

        private void OnDisable()
        {
            YG2.SaveProgress();
        }

        private void savess()
        {
            YG2.onDefaultSaves += () =>
            {
                YG2.saves.Pods = new();
                YG2.saves.Peres = new();
            };
            YG2.onGetSDKData += () =>
            {
                coinsView.View();

                foreach (var v in viewsSmokes)
                    v.SetActive(false);
                viewsSmokes[YG2.saves.activeIdPods].SetActive(true);

                sigar.Load();
                pers.Load();
                add.Load();
            };

            YGInsides.LoadProgress();
        }
    }
}