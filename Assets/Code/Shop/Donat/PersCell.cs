using Audio;
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
        [SerializeField] public Audios audioZone;

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
            if ( YG2.Save.coins < cost)
                return;
            YG2.Save.coins-= cost;

            _counter++;
            countert.text = string.Format(formCount, _counter, maxCount);
            coinsView.View();

            var perss = Instantiate(pers).GetComponent<Pers.Pers>();
            moves.pres.Add(new Moves.MyStruct
            {
                pers = perss,
                add = Random.Range(rangeCost.x, rangeCost.y),
            });
            moves.Ranger(perss.transform);
            
            audioZone.Bye();
        }
    }
}