using Code.Toxiss;
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
        [SerializeField] private Toxis toxis;
        [SerializeField] private ToxView toxisView;
        [SerializeField] private ZoneController zone;
        [SerializeField] private CoinView coinsView;
        [SerializeField] private Active active;
        [SerializeField] private int myId;
        [SerializeField] private TMP_Text costTx;
        [SerializeField] private TMP_Text damageTx;
        [SerializeField] private string damageFr;
        [SerializeField] private TMP_Text addTx;
        [SerializeField] private string addFr;

        [SerializeField] private int cost;
        [SerializeField] private Vector2Int mCoin;
        [SerializeField] private Vector2Int mTox;
        [SerializeField] private float speed;
        [SerializeField] private float size;

        private void Start()
        {
            costTx.text = cost.ToString();
            damageTx.text = string.Format(damageFr, mTox.x, mTox.y);
            addTx.text = string.Format(addFr, mCoin.x, mCoin.y);

       //     YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessHandler;
         //   btBye.onClick.AddListener(() => { YandexGame.BuyPayments("sign"); });
        }


        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "sign") return;

            coins.mult = Random.Range(mCoin.x, mCoin.y);
            toxis.mult = Random.Range(mTox.x, mTox.y);
            zone.speed = speed;
            zone.Reload(size);

            coinsView.View();
            toxisView.View();


            foreach (var vv in active.viewsSmokes)
                vv.SetActive(false);
            active.viewsSmokes[myId].SetActive(true);
        }
    }
}