using Audio;
using Code.Pers;
using UnityEngine;
using YG;
using YG.Insides;
using Random = UnityEngine.Random;

namespace Code
{
    public class Active : MonoBehaviour
    {
        [SerializeField] private ZoneController zone;
        [SerializeField] private Coins coins;

        [SerializeField] private CoinView coinsView;
        [SerializeField] private Moves moves;

        [SerializeField] public Creats creatsZone;
        [SerializeField] public Creats creatsPassive;
        [SerializeField] public Audios audioZone;

        [SerializeField] public GameObject[] viewsSmokes;

        private void Awake()
        {
            
            zone.OnChange += e =>
            {
                if (e)
                {
                    coins.Add();
                    coinsView.View();
                }
            };

            creatsZone.OnBye += cost =>
            {
                if ( YG2.saves.coins < cost)
                    return false;

                YG2.saves.coins -= cost;
                audioZone.Bye();
                return true;
            };
            creatsZone.OnComplete += () =>
            {
                var v = creatsZone.GetStep();
                coins.mult = v.coinMult;
                zone.speed = v.speed;
                zone.Reload(v.size);

                coinsView.View();

                foreach (var vv in viewsSmokes)
                    vv.SetActive(false);
                viewsSmokes[creatsZone._step].SetActive(true);
            };
            creatsZone.Start();


            creatsPassive.OnBye += cost =>
            {
                if ( YG2.saves.coins < cost)
                    return false;
                YG2.saves.coins-= cost;
                audioZone.Bye();
                return true;
            };
            creatsPassive.OnComplete += () =>
            {
                coinsView.View();
                var pers = Instantiate(creatsPassive.view).GetComponent<Pers.Pers>();
                moves.pres.Add(new Moves.MyStruct
                {
                    pers = pers,
                    add = Random.Range(creatsPassive._rangeCost.x, creatsPassive._rangeCost.y),
                });
                moves.Ranger(pers.transform);
            };
            moves.OnAdd += e =>
            {
                YG2.saves.coins += e;
                coinsView.View();
                coinsView.passive.text = string.Format(coinsView.formatPassive, e);
            };

            creatsPassive.Start();

            Saves();
        }

        private void OnDestroy()
        {
//   YG2.SetDefaultSaves();
        YG2.SaveProgress();

        }

        private void Saves()
        {
            YGInsides.LoadProgress();
            
            coinsView.View();
            
            
            foreach (var v in viewsSmokes)
                v.SetActive(false);
            viewsSmokes[YG2.saves.activeIdPods].SetActive(true);
        }
    }
}