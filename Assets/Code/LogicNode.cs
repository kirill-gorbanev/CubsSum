using System.Linq;
using UnityEngine;

namespace Code
{
    public class LogicNode : AccessNode
    {
        [SerializeField] private AccessNode[] gates;

        [SerializeField] bool isAnd;

        public bool IsActive => isAnd ? IsAnd : IsOr;

        private bool IsAnd => gates is { Length: > 0 } && gates.All(e => e.isActive);
        private bool IsOr => gates is { Length: > 0 } && gates.Any(e => e.isActive);
    }
}