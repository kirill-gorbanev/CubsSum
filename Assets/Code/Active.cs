using Audio;
using Code.Pers;
using UnityEngine;

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
                if (coins.coin < cost)
                    return false;

                coins.coin -= cost;
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
                if (coins.coin < cost)
                    return false;
                coins.coin -= cost;
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
                coins.coin += e;
                coinsView.View();
                coinsView.passive.text = string.Format(coinsView.formatPassive, e);
            };

            creatsPassive.Start();

            foreach (var v in viewsSmokes)
                v.SetActive(false);
            viewsSmokes[0].SetActive(true);
        }
    }
}