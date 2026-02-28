using System;
using System.Collections.Generic;


namespace YG
{
    public partial class SavesYG
    {
        public List<int> pods;
        public List< As> Pers;

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