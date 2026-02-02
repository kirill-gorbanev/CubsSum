using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Center : MonoBehaviour
    {
        [SerializeField] private AccessNode[] accessNodes;

        public VoltManage VoltManage { get; private set; } = new();

        public IEnumerable<AccessNode> GetNodes() => accessNodes;
    }
}