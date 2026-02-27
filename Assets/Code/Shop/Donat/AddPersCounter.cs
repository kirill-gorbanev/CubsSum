using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Shop.Donat
{
    public class AddPersCounter : MonoBehaviour
    {
        [SerializeField] private Active active;
        [SerializeField] private Button btBye;
        [SerializeField] private Coins coins;
        [SerializeField] private int cost;
        [SerializeField] private int newMax;
        [SerializeField] private TMP_Text costTx;
        [SerializeField] public Audios audioZone;

        private void Start()
        {
            costTx.text = cost.ToString();
          //  YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessHandler;
          //  btBye.onClick.AddListener(() => { YandexGame.BuyPayments("add_pers"); });
        }


        private void OnPurchaseSuccessHandler(string purchase)
        {
            if (purchase != "add_pers") return;
            
            foreach (var cell in active.creatsPassive._cells)
                cell.counterCell.max = newMax;

            gameObject.SetActive(false);
            audioZone.Bye();
        }
    }
}