using UnityEngine;

namespace Code.Pers
{
    public class Pers : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Animator animator;

        public void Smoke()
        {
            particle.Play();
            animator.SetTrigger("smoke");
        }

        public void Move()
        {
            animator.SetBool("isMove", true);
        }
    }
}