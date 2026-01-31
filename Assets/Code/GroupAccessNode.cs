using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class GroupAccessNode : AccessNode
    {
        [SerializeField] private AccessNode[] group;
        public AccessNode[] Groups =>  group;

        public override IEnumerator<AccessNode> GetEnumerator()
        {
            AccessNode current = this;

            while (current != null && current.isActive)
            {
                if (current is GroupAccessNode groupNode)
                {
                    foreach (var node in groupNode.group)
                    {
                        if (node != null)
                            yield return node;
                    }
                }
                else
                {
                    yield return current;
                }

                current = current.NextNode;
            }
        }
    }
}