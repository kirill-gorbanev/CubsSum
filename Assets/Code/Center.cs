using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Center : MonoBehaviour
    {
        [SerializeField] private AccessNode[] accessNodes;
        
        public IEnumerable<AccessNode> GetNodes() => accessNodes;
    }
}