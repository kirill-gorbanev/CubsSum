using UnityEngine;
using UnityEngine.UI;

namespace Code.Toxiss
{
    public class ToxView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Toxis toxis;

        private void Start()
        {
            View();
        }

        public void View()
        {
            _image.fillAmount = (float)toxis.toxis / toxis.maxTox;
        }
    }
}