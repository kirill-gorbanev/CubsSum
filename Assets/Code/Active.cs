using Code.Effectors;
using Code.Toxiss;
using UnityEngine;
using UnityEngine.Serialization;

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

        [SerializeField] private ByeManage regen;
        [SerializeField] private ByeManage detox;

        [SerializeField] private Creats creatsZone;
        [SerializeField] private Creats creatsPassive;

        private void Awake()
        {
            zone.OnChange += e =>
            {
                coins.Add();
                if (!e)
                    toxis.Add();
                coinsView.View();
                toxisView.View();
            };

            toxis.OnTox += () => { zone.Block(true); };


            regenView.OnBtClicked = () =>
            {
                regen.Bye();
                coinsView.View();
            };
            toxisView.OnBtClicked = () =>
            {
                detox.Bye();
                coinsView.View();
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

                toxis.maxTox *= e;
                toxisView.View();
            };

            creatsZone.OnBye += cost =>
            {
                if (coins.coin < cost)
                    return false;

                coins.coin -= cost;
                var v = creatsZone.GetStep();
                coins.mult = v.coinMult;
                toxis.mult = v.toxMult;
                zone.speed = v.speed;
                zone.Reload(v.step);

                coinsView.View();
                toxisView.View();
                return true;
            };
            creatsZone.Start();


            creatsPassive.OnBye += cost =>
            {
                if (coins.coin < cost)
                    return false;
                coins.coin -= cost;

                coinsView.View();
                return true;
            };
            creatsPassive.Start();
        }
    }
}