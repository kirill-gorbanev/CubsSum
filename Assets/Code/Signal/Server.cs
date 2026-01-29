using System;
using System.Collections.Generic;

namespace Code.Signal
{
   
    public class Server<T> where T : IData
    {
        private readonly Dictionary<Type, List<ISelector<T>>> _cmds = new();

        public void Add(ISelector<T> sel)
        {
            var t = typeof(T);

            if (!_cmds.ContainsKey(t))
                _cmds.Add(t, new List<ISelector<T>>());

            _cmds[t].Add(sel);
        }

        public void Execute(T filt, IFilt own)
        {
            foreach (var cmd in _cmds[filt.GetType()])
                if (cmd.FiltType == own.GetType())
                    cmd.Event(filt);
        }
    }
}