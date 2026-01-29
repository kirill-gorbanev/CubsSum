using System;
using Code.InputSearch;
using Code.Signal;
using UnityEngine;

namespace Code.UI.LocalText
{
  
    [Serializable]
    public struct ItemDestroyText : ISelector<Over>
    {
        [SerializeField] private TextManage textManage;
        [SerializeField] private float delay;
        [SerializeField] private TextSo keyDelet;
        [SerializeField] private TextSo keyLimit;

        public Type FiltType => typeof(CellInteract);

        public void Event(Over own)
        {
            textManage.EnqueueMessage(new TextSignal { key = own.isLimit ? keyLimit : keyDelet, delay = delay });
        }
    }

}