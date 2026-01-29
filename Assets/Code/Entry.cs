using Code.Build;
using Code.InputSearch;
using Code.UI;
using Code.UI.LocalText;
using Code.UI.Scroll;
using UnityEngine;

namespace Code
{
    public class Entry : MonoBehaviour
    {
        [SerializeField] private HandlerClick handlerClick;
        [SerializeField] private CellInteract cellInteract;
        [SerializeField] private CreateCells createCells;

        [SerializeField] private SelectBreak dragBreak;
        [SerializeField] private BreakTower buildTower;

        [SerializeField] private BuildTower towerBuild;
        
        private void Awake()
        {
            handlerClick.ServerDrag.Add(dragBreak);

            handlerClick.ServerUp.Add(dragBreak);
            handlerClick.ServerUp.Add(buildTower);


            cellInteract.ServerUp.Add(towerBuild);


            createCells.ServerUp.Add(cellInteract);
            createCells.ServerDrag.Add(cellInteract);
            createCells.ServerDown.Add(cellInteract);
            TextSelect();
        }

        [Space(4)] [SerializeField] private SetItemUpText s;
        [Space(4)] [SerializeField] private DropItemText d;
        [Space(4)] [SerializeField] private ItemDestroyText id;

        private void TextSelect()
        {
            handlerClick.ServerUp.Add(d);
            
            cellInteract.ServerUp.Add(s);
            
            cellInteract.ServerOver.Add(id);
        }
    }
}