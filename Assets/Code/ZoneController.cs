using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private RectTransform redZone;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Outline outline;
    [SerializeField] public float speed;
    [SerializeField] private float size;
    [SerializeField] private float minSize;


    private float _stripLeftX;
    private float _stripRightX;
    private float _screenX;
    private Vector3 _dir;

    private bool _isRight;

    public event Action<bool> OnChange;

    void Start()
    {
        Vector3[] corners = new Vector3[4];
        redZone.GetWorldCorners(corners);
        _stripLeftX = corners[0].x;
        _stripRightX = corners[2].x;
        _screenX = redZone.position.x;

        greenZone.sizeDelta = new Vector2(minSize, greenZone.sizeDelta.y);
        _dir = greenZone.position;
        greenZone.position =
            Vector3.Lerp(_dir, redZone.position, greenZone.sizeDelta.x / (redZone.sizeDelta.x - minSize));
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(greenZone, cursor.position))
            {
                Click(true);
                outline.effectColor = Color.green;
                StartCoroutine(NoOutline());
            }
            else
            {
                Click(false);
                outline.effectColor = Color.red;
                StartCoroutine(NoOutline());
            }
        }

        Move();
    }

    private IEnumerator NoOutline()
    {
        outline.enabled = true;
        yield return new WaitForSeconds(0.5f);
        outline.enabled = false;
    }

    private void Click(bool isZone)
    {
        greenZone.sizeDelta += Vector2.right * ((isZone ? 1 : -1) * size);

        greenZone.position =
            Vector3.Lerp(_dir, redZone.position, greenZone.sizeDelta.x / (redZone.sizeDelta.x - minSize));

        OnChange?.Invoke(isZone);

        if (greenZone.sizeDelta.x > redZone.sizeDelta.x - minSize)
            greenZone.sizeDelta = new Vector2(redZone.sizeDelta.x - minSize, greenZone.sizeDelta.y);
        if (greenZone.sizeDelta.x < minSize)
            greenZone.sizeDelta = new Vector2(minSize, greenZone.sizeDelta.y);
    }

    private void Move()
    {
        if (_screenX > _stripRightX)
            _isRight = false;
        else if (_screenX < _stripLeftX)
            _isRight = true;

        _screenX += speed * Time.deltaTime * (_isRight ? 1 : -1);
        cursor.position = new Vector3(_screenX, redZone.position.y, redZone.position.z);
    }

    public void Reload(float sizes) => minSize = sizes;
}