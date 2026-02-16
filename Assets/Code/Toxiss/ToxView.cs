using UnityEngine;
using UnityEngine.UI;

namespace Code.Toxiss
{
    public class ToxView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private ZoneController zone;
        [SerializeField] private Toxis toxis;

        private void Start()
        {
            _image.fillAmount = (float)toxis.toxis / toxis.maxTox;
            zone.OnChange += _ => { _image.fillAmount = (float)toxis.toxis / toxis.maxTox; };
        }
    }
}