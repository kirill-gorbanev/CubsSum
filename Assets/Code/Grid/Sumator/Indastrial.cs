using Code.Parametars;
using Code.UI.Tools;
using TMPro;
using UnityEngine;

namespace Code.Grid.Sumator
{
    public class Indastrial : MonoBehaviour
    {
        [SerializeField] private TMP_Text smogTx;
        [SerializeField] private TMP_Text shitTx;

        [SerializeField] private Coins coins;
        [SerializeField] private Indastriality indastrial;
        [SerializeField] private Happy happy;
        [SerializeField] private Ecologi ecologi;
        [SerializeField] private Spawner spawner;
        [SerializeField] private TooltipManager tooltipManager;
        [SerializeField] private TypeRes factoryDetect;
        [SerializeField] private Detect forectDetect;
        [SerializeField] private TypeRes forestDetect;
        [SerializeField] private float multToCoins = 1;
        [SerializeField] private float multToFactory = 1;

        private void Start()
        {
            spawner.OnLoadStep += step =>
            {
                if (step != 6) return;

                var allFactory = Sumator.Detect.AllRezult(factoryDetect, spawner, (p, e) =>
                {
                    if (tooltipManager.tooltips.TryGetValue(p, out var tooltip))
                        tooltip.View(e.ToString());
                });
                allFactory += Sumator.Detect.UseAllResource(factoryDetect, 1, spawner, (p, e) =>
                {
                    if (tooltipManager.tooltips.TryGetValue(p, out var tooltip))
                        tooltip.View(e.ToString());

                });

                coins.Add(allFactory * multToCoins);
                indastrial.Add(allFactory * multToFactory);


                //-----------------------------------------------------
                var f = Sumator.Detect.AllRezult(forestDetect, spawner, (p, e) =>
                {
                    if (tooltipManager.tooltips.TryGetValue(p, out var tooltip))
                        tooltip.View(e.ToString());

                });
                f += Sumator.Detect.UseAllResource(forestDetect, 1, spawner, (p, e) =>
                {
                    if (tooltipManager.tooltips.TryGetValue(p, out var tooltip))
                        tooltip.View(e.ToString());

                });

                if (f <= allFactory)
                {
                    allFactory -= f;
                    f = 0;
                    happy.Damage(allFactory);
                }
                else
                {
                    f -= allFactory;
                    allFactory = 0;
                    ecologi.Add(f);
                }

                smogTx.text = allFactory.ToString();
                shitTx.text = f.ToString();
            };
        }

        private bool Detect(Vector2Int d)
        {
            var g = spawner.grid;
            return d.x < 0 || d.x >= g.x || d.y < 0 || d.y >= g.y;
        }
    }
}