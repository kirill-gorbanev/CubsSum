using Code.Pers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Random = UnityEngine.Random;

namespace Code.Shop.Donat
{
    public class PersCell : MonoBehaviour
    {
        [SerializeField] private Button btBye;
        [SerializeField] private GameObject pers;
        [SerializeField] private Coins coins;
        [SerializeField] private CoinView coinsView;
        [SerializeField] private Moves moves;
        [SerializeField] private int cost;
        [SerializeField] private int maxCount;
        [SerializeField] private Vector2Int rangeCost;
        [SerializeField] private TMP_Text costTx;
        [SerializeField] private TMP_Text countert;
        [SerializeField] private string formCount;
        [SerializeField] private TMP_Text passiveTx;
        [SerializeField] private string formPassive;

        private int _counter;

        private void Start()
        {
            costTx.text = cost.ToString();
            passiveTx.text = string.Format(formCount, rangeCost.x, rangeCost.y);
            countert.text = string.Format(formCount, _counter, maxCount);
            _counter = 1;
   
         //   YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessHandler;
        //    btBye.onClick.AddListener(() => { YandexGame.BuyPayments("pers"); });
        }


        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "pers") return;
            Bye();
        }

        private void Bye()
        {
            if (_counter >= maxCount)
                return;
            if (coins.coin < cost)
                return;
            coins.coin -= cost;

            _counter++;
            countert.text = string.Format(formCount, _counter, maxCount);
            coinsView.View();
            moves.pres.Add(new Moves.MyStruct
            {
                pers = Instantiate(pers).GetComponent<Pers.Pers>(),
                add = Random.Range(rangeCost.x, rangeCost.y),
            });
            moves.Ranger();
        }
    }
}