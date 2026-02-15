using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private RectTransform redZone;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private float speed;
    [SerializeField] private float size;
    [SerializeField] private float minSize;

    public int _click;

    private bool isRight;

    private float stripLeftX;
    private float stripRightX;
    private float screenX;

    void Start()
    {
        Vector3[] corners = new Vector3[4];
        redZone.GetWorldCorners(corners);
        stripLeftX = corners[0].x;
        stripRightX = corners[2].x;
        screenX = redZone.position.x;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(greenZone, cursor.position))
            {
                _click++;
                greenZone.sizeDelta += Vector2.right * size;
                
                if (greenZone.sizeDelta.x > redZone.sizeDelta.x - minSize)
                    greenZone.sizeDelta = new Vector2(redZone.sizeDelta.x- minSize, greenZone.sizeDelta.y);
            }
            else
            {
                _click--;
                greenZone.sizeDelta -= Vector2.right * size;
                if (greenZone.sizeDelta.x < minSize)
                    greenZone.sizeDelta = new Vector2(minSize, greenZone.sizeDelta.y);
            }
        }

        if (screenX > stripRightX)
            isRight = false;
        else if (screenX < stripLeftX)
            isRight = true;

        screenX += speed * Time.deltaTime * (isRight ? 1 : -1);
        cursor.position = new Vector2(screenX, redZone.position.y);
    }
}