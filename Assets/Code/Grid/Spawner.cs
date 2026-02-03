using UnityEngine;

namespace Code.Grid
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Vector2Int grid;
        [SerializeField] private Vector2 size;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color a;
        [SerializeField] private Color b;

        private void Start()
        {
            for (int i = 0; i < grid.x; i++)
            {
                for (int j = 0; j < grid.y; j++)
                {
                    var sr = Instantiate(spriteRenderer, (Vector2)transform.position + new Vector2(i, j) * size,
                        Quaternion.identity);
                    if ((j + (i % 2 == 0 ? 1 : 0)) % 2 == 0)
                        sr.color = a;
                    else
                        sr.color =b;
                }
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < grid.x; i++)
            {
                for (int j = 0; j < grid.y; j++)
                {
                    Gizmos.DrawCube((Vector2)transform.position + new Vector2(i, j) * size, Vector2.one/2f);
                }
            }
        }
    }
}