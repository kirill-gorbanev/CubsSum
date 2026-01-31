using UnityEngine;

public class PlacementExample : MonoBehaviour
{
    public TetrisGrid grid;
    
    void Start()
    {
        // Попытка разместить фигуру T в позиции (4, 0)
        Vector2Int position = new Vector2Int(4, 0);
        
        if (grid.CanPlace(TetrominoShape.T, position))
        {
            grid.Place(TetrominoShape.T, position, 1); // ID=1 для синего цвета
            Debug.Log("Фигура размещена!");
        }
        else
        {
            Debug.Log("Нет места для размещения");
        }
    }
}