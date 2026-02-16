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

        [SerializeField] private CoinView coinsView;
        [SerializeField] private ToxView toxisView;
        [SerializeField] private RegenView regenView;

        private void Awake()
        {
            zone.OnChange += _ =>
            {
                coins.Add();
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
        }
    }
}