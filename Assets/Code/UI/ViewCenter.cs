using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ViewCenter : MonoBehaviour
    {
        [SerializeField] private AdditiveSceneLoader loader;
        [SerializeField] private TMP_Text textWC;
        [SerializeField] private TMP_Text textWT;
        [SerializeField] private Toggle btPrefab;
        [SerializeField] private Transform parent;

        private void Awake()
        {
            loader.     OnLoad += Load;
        }

        private void Load()
        {
            foreach (var ac in loader.AllAccessNodes)
            {
                var p = Instantiate(btPrefab, parent);
                p.isOn = ac.isActive;
                p.onValueChanged.AddListener(e =>
                {
                    ac.isActive = e;
                    Execute();
                });
                p.GetComponentInChildren<TMP_Text>().text = ac.name;
            }

            loader.Center.VoltManage.wCurrentAction += e => textWC.text = e.ToString();
            loader.Center.VoltManage.wTotalAction += e => textWT.text = e.ToString();
        }

        private void Execute()
        {
            _isActive = false;
            loader.Center.VoltManage.ResetVoltCurrent();

            AccessNodeTraversal.TraverseAll(loader.Center);

            _isActive = true;
            StartCoroutine(GetAccessNodes());
        }

        private bool _isActive;

        private IEnumerator GetAccessNodes()
        {
            while (_isActive)
            {
                yield return new WaitForSeconds(10); // - seconds == hour 
                loader.Center.VoltManage.AddVoltOnCurrent();
            }
        }
    }


    public static class AccessNodeTraversal
    {
        public static void TraverseAll(Center center)
        {
            foreach (AccessNode startNode in center.GetNodes())
            {
                TraverseChain(center, startNode);
            }
        }


        private static void TraverseChain(Center center, AccessNode node)
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
                        var m = member.Mech();
                        center.VoltManage.AddVoltCurrent(m);
                        TraverseChain(center, member);
                    }
                }
            }
            else
            {
                Debug.Log($"Processing node: {node.name}");
                var m = node.Mech();
                center.VoltManage.AddVoltCurrent(m);
            }


            TraverseChain(center, node.NextNode);
        }
    }
}