using UnityEngine;
using UnityEngine.UI;

namespace Code.Shop
{
    public class CellShop : MonoBehaviour
    {
        public Button bt;
        public Transform parentView;

        public RectTransform rectTransform;

        private void LateUpdate()
        {
            parentView.gameObject.SetActive(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, parentView.position));
        }
    }
}