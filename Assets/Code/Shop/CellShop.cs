using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Shop
{
    public class CellShop : MonoBehaviour
    {
        public Button bt;
        public TMP_Text cost;
        public TMP_Text damage;
        public TMP_Text add;
        public string formatAdd;
        public Transform parentView;
        public Image bgView;

        [HideInInspector] public RectTransform rectTransform;

        private void LateUpdate()
        {
            parentView.gameObject.SetActive(
                RectTransformUtility.RectangleContainsScreenPoint(rectTransform, parentView.position));
        }
    }
}