using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code.UI.LocalText
{
    public class TextManage : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private int lang;

        private readonly Queue<TextSignal> _messageQueue = new();
        private bool _isProcessing = false;

        public void EnqueueMessage(TextSignal signal)
        {
            _messageQueue.Enqueue(signal);
            if (!_isProcessing)
            {
                StartCoroutine(ProcessMessages());
            }
        }

        private IEnumerator ProcessMessages()
        {
            _isProcessing = true;

            while (_messageQueue.Count > 0)
            {
                var currentSignal = _messageQueue.Dequeue();

                text.text =currentSignal.key.texts[lang];

                yield return new WaitForSeconds(currentSignal.delay);
            }

            text.text = "";
            _isProcessing = false;
        }
    }
}