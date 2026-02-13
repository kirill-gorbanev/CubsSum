using UnityEngine;

namespace Code.Grid.Sumator
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;

        [SerializeField] private TypeCell[] typesFind;

        private void Start()
        {
            spawner.OnLoad += FindEnergy;
        }

        private void FindEnergy()
        {
            var r = 0f;
            
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];

                    foreach (TypeCell cell in typesFind)
                    {
                        if (c.typeCell == cell)
                        {
                            r += c.value;
                            break;
                        }
                    }
                }
            }
            
            
        }
    }
}