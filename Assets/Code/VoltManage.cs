using System;
using System.Collections.Generic;
using System.Linq;

namespace Code
{
    public class VoltManage
    {
        HashSet<ActuatingMechanism> wCurrent = new();
        int wTotal;

        public event Action<int> wCurrentAction;
        public event Action<int> wTotalAction;

        public void ResetVoltCurrent()
        {
            wCurrent.Clear();
            wCurrentAction?.Invoke(0);
        }

        public void AddVoltCurrent(ActuatingMechanism mech)
        {
            wCurrent.Add(mech);
            wCurrentAction?.Invoke(wCurrent.Sum(e => e.w));
        }

        public void AddVoltCurrent(ActuatingMechanism[] mech)
        {
            foreach (var item in mech)
                wCurrent.Add(item);
            wCurrentAction?.Invoke(wCurrent.Sum(e => e.w));
        }

        public void AddVoltTotal(int w)
        {
            wTotal += w;
            wTotalAction?.Invoke(wTotal);
        }

        public void AddVoltOnCurrent()
        {
            wTotal += wCurrent.Sum(e => e.w);
            wTotalAction?.Invoke(wTotal);
        }
    }
}