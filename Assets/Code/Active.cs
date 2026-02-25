using System.Collections;
using Code.Effectors;
using Code.Pers;
using Code.Toxiss;
using UnityEngine;

namespace Code
{
    public class Active : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;
        [SerializeField] private Coins coins;
        [SerializeField] private Toxis toxis;

        [SerializeField] private CoinView coinsView;
        [SerializeField] private ToxView toxisView;
        [SerializeField] private RegenView regenView;
        [SerializeField] private Moves moves;

        [SerializeField] private ByeManage regen;
        [SerializeField] private ByeManage detox;

        [SerializeField] public Creats creatsZone;
        [SerializeField] public Creats creatsPassive;

        [SerializeField] public GameObject[] viewsSmokes;

        private void Awake()
        {
            zone.OnChange += e =>
            {
                coins.Add();
                if (!e)
                {
                    toxis.Add();

                    if (!_isDetox)
                        StartCoroutine(Detox());
                    _isDetox = false;
                }

                coinsView.View();
                toxisView.View();
            };

            toxis.OnTox += () => { zone.Block(true); };


            regenView.OnBtClicked = () =>
            {
                regen.Bye();
                coinsView.View();
                regenView.View(regen.Cost.ToString());
            };
            toxisView.OnBtClicked = () =>
            {
                detox.Bye();
                coinsView.View();
                toxisView.View(detox.Cost.ToString(), toxis.mult.ToString());
            };
            regen.OnByeCompleted += _ =>
            {
                zone.Block(false);

                toxis.toxis = 0;
                toxisView.View();
            };
            detox.OnByeCompleted += e =>
            {
                zone.Block(false);

                toxis.maxTox *= 2;
                toxisView.View();
            };

            creatsZone.OnBye += cost =>
            {
                if (coins.coin < cost)
                    return false;

                coins.coin -= cost;
                return true;
            };
            creatsZone.OnComplete += () =>
            {
                var v = creatsZone.GetStep();
                coins.mult = v.coinMult;
                toxis.mult = v.toxMult;
                zone.speed = v.speed;
                zone.Reload(v.size);

                coinsView.View();
                toxisView.View();
                toxisView.View(detox.Cost.ToString(), toxis.mult.ToString());


                foreach (var vv in viewsSmokes)
                    vv.SetActive(false);
                viewsSmokes[creatsZone._step].SetActive(true);
            };
            creatsZone.Start();


            creatsPassive.OnBye += cost =>
            {
                if (coins.coin < cost)
                    return false;
                coins.coin -= cost;

                return true;
            };
            creatsPassive.OnComplete += () =>
            {
                coinsView.View();
                moves.pres.Add(new Moves.MyStruct
                {
                    pers = Instantiate(creatsPassive.view).GetComponent<Pers.Pers>(),
                    add = Random.Range(creatsPassive._rangeCost.x, creatsPassive._rangeCost.y),
                });
                moves.Ranger();
            };
            moves.OnAdd += e =>
            {
                coins.coin += e;
                coinsView.View();
                coinsView.passive.text = string.Format(coinsView.formatPassive, e);
            };

            regenView.View(regen.Cost.ToString());
            toxisView.View(detox.Cost.ToString(), toxis.mult.ToString());
            creatsPassive.Start();

            foreach (var v in viewsSmokes)
                v.SetActive(false);
            viewsSmokes[0].SetActive(true);
        }

        private bool _isDetox;

        private IEnumerator Detox()
        {
            yield return new WaitForSeconds(5f);
            toxis.toxis-=10;
            _isDetox = true;
        }
    }
}