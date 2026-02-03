using Code.Grid.Form;
using UnityEngine;

namespace Code.UI
{
    [CreateAssetMenu]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] public FormConstruct[] forms;
        [SerializeField] public Vector3 size;
    }
}