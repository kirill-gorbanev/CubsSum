using Unity.VisualScripting;
using UnityEngine;

namespace Code
{
    public class Anim : MonoBehaviour
    {
        public ParticleSystem particle;
        public Animator animator;
        public ZoneController zone;
        public float time;
        private float _timeDetect;
        private bool isanim;

        private void Start()
        {
            zone.OnChange += b =>
            {
                isanim = false;
                animator.SetBool("Up", isanim);

                _timeDetect = time;
            };
        }

        private void Update()
        {
            if (isanim)
                return;

            if (_timeDetect >= 0)
            {
                _timeDetect -= Time.deltaTime;
            }
            else
            {
                isanim = true;
                Smoke();
            }
        }

        private void Smoke()
        {
            particle.Play();
            animator.SetBool("Up", isanim);
        }
    }
}