using System;
using Code.InputSearch;
using Code.Signal;
using UnityEngine;

namespace Code.UI.LocalText
{
    [Serializable]
    public struct SetItemUpText : ISelector<Up>
    {
        [SerializeField] private TextManage textManage;
        [SerializeField] private float delay;
        [SerializeField] private TextSo key;

        public Type FiltType => typeof(CellInteract);


        public void Event(Up own)
        {
            if (own.transform != null)
                textManage.EnqueueMessage(new TextSignal { key = key, delay = delay });
        }
    }

}