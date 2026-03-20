
using System;
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;
        
        
        public List<int> Pods;
        public List<As> Peres; 

        public int activeIdPods;

        public int count;
        public int last;
        public int step;
        public double coins;
        public bool isSounds = true;
        
        public bool isByeSigar;
        public bool isByePers;
        public bool isByeAdd;

    }
    
    [Serializable]
    public class As
    {
        public int id;
        public int count;
    }
}
