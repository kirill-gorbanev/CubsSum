using System.Linq;
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
        [SerializeField] private int maxCount;
        [SerializeField] private Vector2Int rangeCost;
        [SerializeField] private TMP_Text costTx;
        [SerializeField] private TMP_Text countert;
        [SerializeField] private string formCount;
        [SerializeField] private TMP_Text passiveTx;
        [SerializeField] private string formPassive;
        [SerializeField] public Audios audioZone;
        public int id;

        private void Start()
        {
            //  costTx.text = cost.ToString();
            passiveTx.text = string.Format(formCount, rangeCost.x, rangeCost.y);


            if (YandexGame.savesData.Peres != null)
            {
                var s = YandexGame.savesData.Peres.FirstOrDefault(e => e.id == id);

                if (s != null)
                    countert.text = string.Format(formCount, s.count, maxCount);
            }

            Load();

            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessHandler;
            btBye.onClick.AddListener(() =>
            {
               
                if (YandexGame.savesData.Peres != null)
                {
                    var s = YandexGame.savesData.Peres.FirstOrDefault(e => e.id == id);

                    if (s == null || s.count < maxCount)
                        YandexGame.BuyPayments("pers");
                }
            });

            YandexGame.ConsumePurchaseByID("pers");
        }


        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "pers") return;
            Bye();
        }

        private void Bye()
        {
            if (YandexGame.savesData.Peres != null)
            {
                var s = YandexGame.savesData.Peres.FirstOrDefault(e => e.id == id);

                if (s == null)
                {
                    s = new() { id = id, count = 0 };
                    YandexGame.savesData.Peres.Add(s);
                }

                if (s.count >= maxCount)
                    return;

                s.count++;

                YandexGame.savesData.isByePers = true;
                Load();

                audioZone.Bye();
            }
        }

        private int active = 0;

        public void Load()
        {
            if (YandexGame.savesData.Peres != null)
            {
                var s = YandexGame.savesData.Peres.FirstOrDefault(e => e.id == id);

                if (s != null)
                    if (YandexGame.savesData.isByePers)
                    {
                        countert.text = string.Format(formCount, s.count, maxCount);
                        coinsView.View();
                        if (active < s.count)
                            for (int i = 0; i < s.count - active; i++)
                            {
                                var perss = Instantiate(pers).GetComponent<Pers.Pers>();
                                moves.pres.Add(new Moves.MyStruct
                                {
                                    pers = perss,
                                    add = Random.Range(rangeCost.x, rangeCost.y),
                                });
                                moves.Ranger(perss.transform);
                                active++;
                            }
                    }
            }
        }
    }
}