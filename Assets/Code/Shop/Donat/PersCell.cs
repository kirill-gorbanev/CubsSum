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


            if (YG2.saves.Peres != null)
            {
                var s = YG2.saves.Peres.FirstOrDefault(e => e.id == id);

                if (s != null)
                    countert.text = string.Format(formCount, s.count, maxCount);
            }

            Load();

            YG2.onPurchaseSuccess += OnPurchaseSuccessHandler;
            btBye.onClick.AddListener(() =>
            {
                if (YG2.saves.Peres != null)
                {
                    var s = YG2.saves.Peres.FirstOrDefault(e => e.id == id);

                    if (s == null || s.count < maxCount)
                        YG2.BuyPayments("pers");
                }
            });

            YG2.onPurchaseSuccess += e =>
            {
                if (e == "pers")
                    Load();
            };
            YG2.ConsumePurchaseByID("pers");
        }


        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "pers") return;
            Bye();
        }

        private void Bye()
        {
            if (YG2.saves.Peres != null)
            {
                var s = YG2.saves.Peres.FirstOrDefault(e => e.id == id);

                if (s == null)
                {
                    s = new() { id = id, count = 0 };
                    YG2.saves.Peres.Add(s);
                }

                if (s.count >= maxCount)
                    return;

                s.count++;

                YG2.saves.isByePers = true;
                YG2.SaveProgress();
                Load();

                audioZone.Bye();
            }
        }

        private int active = 0;

        public void Load()
        {
            if (YG2.saves.Peres != null)
            {
                var s = YG2.saves.Peres.FirstOrDefault(e => e.id == id);

                if (s != null)
                    if (YG2.saves.isByePers)
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