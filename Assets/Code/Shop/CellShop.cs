using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Shop
{
    public class CellShop : MonoBehaviour
    {
        public Button bt;
        public TMP_Text cost;
        public TMP_Text add;
        public string formatAdd;
        public Transform parentView;
        public Image bgView;
        public Image bgCell;
        public GameObject obj;

        public CounterCell counterCell;
        public int costValue;

        [HideInInspector] public RectTransform rectTransform;

        public void Active()
        {
            if (obj != null)
            {
                obj.SetActive(false);
                cost.gameObject.SetActive(false);
            }
        }

        private void LateUpdate()
        {
            parentView.gameObject.SetActive(
                RectTransformUtility.RectangleContainsScreenPoint(rectTransform, parentView.position));
        }
    }
}