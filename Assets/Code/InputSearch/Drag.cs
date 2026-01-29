using Code.Signal;
using UnityEngine;

namespace Code.InputSearch
{
 
    public struct Drag : IData
    {
        public Transform item;
        public Vector2 origin;
    }

}