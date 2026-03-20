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

            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessHandler;
            btBye.onClick.AddListener(() =>
            {      

                if (!YandexGame.savesData.isByeSigar)
                    YandexGame.BuyPayments("sigar");
                else
                {
                    YandexGame.savesData.activeIdPods = myId;
                    Load();
                }
            });
            YandexGame.ConsumePurchaseByID("sigar");
        }

        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "sigar") return;
            YandexGame.savesData.isByeSigar = true;
            YandexGame.savesData.activeIdPods = myId;
            Load();

            audioZone.Bye();
        }

        public void Load()
        {
            if (YandexGame.savesData.activeIdPods == myId && YandexGame.savesData.isByeSigar)
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