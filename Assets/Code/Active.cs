using Code.Effectors;
using Code.Toxiss;
using UnityEngine;

namespace Code
{
    public class Active : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;
        [SerializeField] private Coins coins;
        [SerializeField] private Toxis toxis;
        [SerializeField] private Regen regen;
        [SerializeField] private Creats creatsView;

        [SerializeField] private CoinView coinsView;
        [SerializeField] private ToxView toxisView;
        [SerializeField] private RegenView regenView;

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

            regen.OnRegen += () =>
            {
                zone.Block(false);

                toxis.toxis = 0;
                toxisView.View();

                regenView.View();
            };
            regenView.OnBtClicked = () => { regen.Bye(); };

            creatsView.OnBye += cost =>
            {
                if (coins.coin < cost)
                    return false;

                coins.coin -= cost;
                var v = creatsView.GetStep();
                coins.mult = v.coinMult;
                toxis.mult = v.toxMult;
                zone.speed = v.speed;
                zone.Reload(v.step);

                coinsView.View();
                toxisView.View();
                return true;
            };
        }
    }
}