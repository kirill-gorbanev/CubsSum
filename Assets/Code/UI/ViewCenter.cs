using UnityEngine;

namespace Code.UI
{
    public class ViewCenter : MonoBehaviour
    {
        [SerializeField] private Center center;

        private void Start()
        {
            AccessNodeTraversal.TraverseAll(center);
        }
    }


    public static class AccessNodeTraversal
    {
        public static void TraverseAll(Center center)
        {
            foreach (AccessNode startNode in center.GetNodes())
            {
                TraverseChain(startNode);
            }
        }


        private static void TraverseChain(AccessNode node)
        {
            if (node == null || !node.isActive)
                return;

            if (node is GroupAccessNode groupNode)
            {
                foreach (var member in groupNode.Groups)
                {
                    if (member != null)
                    {
                        Debug.Log($"Processing group member: {member.name}");
                        member.Mech();
                        TraverseChain(member);
                    }
                }
            }
            else
            {
                Debug.Log($"Processing node: {node.name}");
                node.Mech();
            }


            TraverseChain(node.NextNode);
        }
    }
}