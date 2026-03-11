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

        private void Start()
        {
            // costTx.text = cost.ToString();
            Load();

            YG2.onPurchaseSuccess += OnPurchaseSuccessHandler;
            btBye.onClick.AddListener(() =>
            {            YG2.OpenAuthDialog();

                if (!YG2.saves.isByeAdd)
                    YG2.BuyPayments("add_pers");
            });
            YG2.ConsumePurchaseByID("add_pers");
        }


        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "add_pers") return;

            YG2.saves.isByeAdd = true;
            YG2.SaveProgress();
            Load();
            audioZone.Bye();
        }

        public void Load()
        {
            if (YG2.saves.isByeAdd)
            {
                foreach (var cell in active.creatsPassive._cells)
                    cell.counterCell.max = newMax;

                gameObject.SetActive(false);
            }
        }
    }
}