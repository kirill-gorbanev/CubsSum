using System;

namespace Code.Signal
{
  
    public interface ISelector<T> where T : IData
    {
        public Type FiltType { get; }

        public void Event(T own);
    }

}