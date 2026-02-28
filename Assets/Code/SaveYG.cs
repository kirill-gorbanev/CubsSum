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
        public double coins;
        public bool isSounds = true;
    }

    [Serializable]
    public class As
    {
        public int id;
        public int count;
    }
}