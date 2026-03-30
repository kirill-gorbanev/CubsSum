using System.Linq;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Code.Shop.Donat
{
    public class AddPersCounter : MonoBehaviour
    {
        [SerializeField] private Active active;
        [SerializeField] private Button btBye;
        [SerializeField] private Coins coins;
        [SerializeField] private int newMax;
        [SerializeField] private TMP_Text costTx;
        [SerializeField] public Audios audioZone;
        [SerializeField] public GameObject cell;
        [SerializeField] public Image icon;

        private void Start()
        {
            var p = YandexGame.purchases.First(e => e.id == "add_pers");
            costTx.text =p.priceValue;
            EX.LoadImageFromUrlAsync(p.currencyImageURL, icon);

            Load();

            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessHandler;
            btBye.onClick.AddListener(() =>
            {         
                if (!YandexGame.savesData.isByeAdd)
                    YandexGame.BuyPayments("add_pers");
            });
            YandexGame.ConsumePurchaseByID("add_pers");
        }


        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "add_pers") return;

            YandexGame.savesData.isByeAdd = true;
            Load();
            audioZone.Bye();
        }

        public void Load()
        {
            if (YandexGame.savesData.isByeAdd)
            {
                foreach (var cell in active.creatsPassive._cells)
                    cell.counterCell.max = newMax;

                cell.SetActive(false);
            }
        }
    }
}