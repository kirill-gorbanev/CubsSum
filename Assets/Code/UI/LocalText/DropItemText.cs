using System;
using Code.Build;
using Code.InputSearch;
using Code.Signal;
using UnityEngine;

namespace Code.UI.LocalText
{

    [Serializable]
    public struct DropItemText : ISelector<Up>
    {
        [SerializeField] private TextManage textManage;
        [SerializeField] private float delay;
        [SerializeField] private TextSo key;

        public Type FiltType => typeof(DragBreak);

        public void Event(Up own)
        {
            textManage.EnqueueMessage(new TextSignal { key = key, delay = delay });
        }
    }

}