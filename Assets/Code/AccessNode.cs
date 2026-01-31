using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class AccessNode : MonoBehaviour, IEnumerable<AccessNode>
    {
        [SerializeField] private ActuatingMechanism[] mechanisms;
        [SerializeField] private AccessNode accessNodes;

        public bool isActive;
        
        public AccessNode NextNode => accessNodes;
        
        public virtual IEnumerator<AccessNode> GetEnumerator()
        {
            var current = this;
            while (current != null && current.isActive)
            {
                yield return current;
                current = current.accessNodes;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Mech()
        {
            foreach (var m in mechanisms)
            {
                Debug.Log(m);
            }
        }
    }
}