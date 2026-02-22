using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI.Tools
{
    public class Tools : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        public void Sub(UnityAction action) => _button.onClick.AddListener(action);

        public void InActive()
        {
            gameObject.SetActive(false);
        }

        public void View(string value)
        {
            gameObject.SetActive(true);
            _text.text = value;
        }

        private bool _c;
        public void Find()
        {
            if (_c) return;
            _c = true;
            gameObject.SetActive(true);

            StartCoroutine(Select());
        }

        private IEnumerator Select()
        {
            var i = transform.localScale;
            transform.localScale += Vector3.one;
            yield return new WaitForSeconds(0.5f);
            transform.localScale = i;
            _c = false;
        }
    }
}