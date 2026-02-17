using TMPro;
using UnityEngine;

namespace Code.Grid.Sumator
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private Detect energyType;

        public float _energy { get; set; }

        private void Start()
        {
            spawner.OnLoadStep += FindEnergy;
        }

        private void FindEnergy(int step)
        {
            if (step != 1) return;
            
            energyType.Check();
            _energy = energyType._energy;
            energyText.text =_energy.ToString();
        }
    }
}