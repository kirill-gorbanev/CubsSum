using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 20;
    
    // 0 = пусто, >0 = ID фигуры/цвета
    private int[,] grid;
    
    private void Awake()
    {
        grid = new int[width, height];
    }
    
    // Проверка, свободны ли все клетки под фигуру
    public bool CanPlace(TetrominoShape shape, Vector2Int position)
    {
        foreach (var offset in shape.cells)
        {
            int x = position.x + offset.x;
            int y = position.y + offset.y;
            
            // Проверка границ
            if (x < 0 || x >= width || y < 0 || y >= height)
                return false;
            
            // Проверка занятости
            if (grid[x, y] != 0)
                return false;
        }
        return true;
    }
    
    // Размещение фигуры на поле
    public bool Place(TetrominoShape shape, Vector2Int position, int pieceId)
    {
        if (!CanPlace(shape, position))
            return false;
        
        foreach (var offset in shape.cells)
        {
            int x = position.x + offset.x;
            int y = position.y + offset.y;
            grid[x, y] = pieceId;
        }
        return true;
    }
    
    // Очистка клетки (для удаления фигуры)
    public void ClearCell(Vector2Int position)
    {
        if (position.x >= 0 && position.x < width && 
            position.y >= 0 && position.y < height)
        {
            grid[position.x, position.y] = 0;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (grid == null) return;
    
        float cellSize = 1f;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = transform.position + new Vector3(x * cellSize, y * cellSize, 0);
                Gizmos.color = grid[x, y] == 0 ? Color.gray : Color.blue;
                Gizmos.DrawCube(pos, Vector3.one * cellSize * 0.9f);
            }
        }
    }
}

[System.Serializable]
public class TetrominoShape
{
    public string name;
    public Vector2Int[] cells; // Относительные координаты клеток от центра/корня
    
    // Статические шаблоны всех 7 фигур
    public static TetrominoShape I = new TetrominoShape
    {
        name = "I",
        cells = new Vector2Int[] { 
            new Vector2Int(-1, 0), 
            new Vector2Int(0, 0), 
            new Vector2Int(1, 0), 
            new Vector2Int(2, 0) 
        }
    };
    
    public static TetrominoShape O = new TetrominoShape
    {
        name = "O",
        cells = new Vector2Int[] { 
            new Vector2Int(0, 0), 
            new Vector2Int(1, 0), 
            new Vector2Int(0, 1), 
            new Vector2Int(1, 1) 
        }
    };
    
    public static TetrominoShape T = new TetrominoShape
    {
        name = "T",
        cells = new Vector2Int[] { 
            new Vector2Int(-1, 0), 
            new Vector2Int(0, 0), 
            new Vector2Int(1, 0), 
            new Vector2Int(0, 1) 
        }
    };
    
    // Добавьте остальные: L, J, S, Z по аналогии
}