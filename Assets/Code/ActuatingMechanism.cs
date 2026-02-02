using UnityEngine;
using UnityEngine.Events;

namespace Code
{
    public class ActuatingMechanism : MonoBehaviour
    {
        public UnityEvent<bool> call;
        public int w;
    }
}