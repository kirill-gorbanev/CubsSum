using System;
using UnityEngine;

namespace Code.Grid.Sumator
{
    [Serializable]
    public class Detect
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TypeRes energyType;
        
        public float _energy { get; set; }

        
        public void Check()
        {
            var g = spawner.grid;
            for (int i = 0; i < g.x; i++)
            {
                for (int j = 0; j < g.y; j++)
                {
                    var c = spawner.CellsActive[i, j];
                    
                    var valR = c.typeCell.res.Range;
                    foreach (var point in c.typeCell.pointers.pointsDetect)
                    {
                        var d = new Vector2Int((int)point.localPosition.x + i, (int)point.localPosition.y + j);

                        if (d.x < 0 || d.x >= g.x || d.y < 0 || d.y >= g.y)
                            continue;
                        var ce = spawner.CellsActive[d.x, d.y];
                        foreach (var comp in c.typeCell.compatible)
                        {
                            if (comp.id == ce.typeCell)
                            {
                                valR = comp.connect.GetValue(valR);
                            }
                        }

                    }

                    if (c.typeCell.res == energyType)
                        _energy += valR;
                    else
                        spawner.CellsActive[i, j].moment = valR;
                }
            }

        }
    }
}