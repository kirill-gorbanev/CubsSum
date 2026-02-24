using Code.UI.Tools;
using TMPro;
using UnityEngine;

namespace Code.Grid.Sumator
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TMP_Text allEnergyText;
        [SerializeField] private TypeRes energyType;
        [SerializeField] private TooltipManager tooltipManager;

        public float _energy { get; set; }

        private void Start()
        {
            spawner.OnLoadStep += FindEnergy;
        }

        private void FindEnergy(int step)
        {
            if (step != 1) return;

            _energy = Detect.CheckAllResource(energyType, spawner, (p, e) =>
            {
                if (tooltipManager.tooltips.TryGetValue(p, out var tooltip))
                    tooltip.View(e.ToString());
            });
            
            allEnergyText.text = _energy.ToString();
        }
    }
}