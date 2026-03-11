using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Code.Shop.Donat
{
    public class Sigar : MonoBehaviour
    {
        [SerializeField] private Button btBye;
        [SerializeField] private Coins coins;
        [SerializeField] private ZoneController zone;
        [SerializeField] private CoinView coinsView;
        [SerializeField] private Active active;
        [SerializeField] private int myId;
        [SerializeField] private TMP_Text costTx;
        [SerializeField] private TMP_Text damageTx;
        [SerializeField] private string damageFr;
        [SerializeField] private TMP_Text addTx;
        [SerializeField] private string addFr;

        [SerializeField] private Vector2Int mCoin;
        [SerializeField] private Vector2Int mTox;
        [SerializeField] private float speed;
        [SerializeField] private float size;

        [SerializeField] public Audios audioZone;

        private void Start()
        {
            //   costTx.text = cost.ToString();
            damageTx.text = string.Format(damageFr, mTox.x, mTox.y);
            addTx.text = string.Format(addFr, mCoin.x, mCoin.y);

            Load();

            YG2.onPurchaseSuccess += OnPurchaseSuccessHandler;
            btBye.onClick.AddListener(() =>
            {            YG2.OpenAuthDialog();

                if (!YG2.saves.isByeSigar)
                    YG2.BuyPayments("sigar");
                else
                {
                    YG2.saves.activeIdPods = myId;
                    YG2.SaveProgress();
                    Load();
                }
            });
            YG2.ConsumePurchaseByID("sigar");
        }

        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "sigar") return;
            YG2.saves.isByeSigar = true;
            YG2.saves.activeIdPods = myId;
            YG2.SaveProgress();
            Load();

            audioZone.Bye();
        }

        public void Load()
        {
            if (YG2.saves.activeIdPods == myId && YG2.saves.isByeSigar)
            {
                costTx.text = "";
                coins.mult = Random.Range(mCoin.x, mCoin.y);
                zone.speed = speed;
                zone.Reload(size);

                coinsView.View();

                foreach (var vv in active.viewsSmokes)
                    vv.SetActive(false);
                active.viewsSmokes[myId].SetActive(true);
            }
        }
    }
}