using System;
using System.Collections.Generic;

namespace YG
{
    public partial class SaveYG
    {
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