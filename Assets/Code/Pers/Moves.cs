using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Pers
{
    public class Moves : MonoBehaviour
    {
        [SerializeField] private Transform point;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector2 size;
        [SerializeField] private Vector3 rot;
        [SerializeField] private float speed;

        public List<MyStruct> pres = new();

        private float _time;
        private float _timeStop;
        [SerializeField] private float _sec = 1f;
        
        private List<Vector3> start = new();

        public event Action<int> OnAdd;

        [Serializable]
        public class MyStruct
        {
            public Pers pers;
            public Vector3 target;
            public int add;

            public bool IsNext => Vector3.Distance(pers.transform.position, target) < 1f;

            public void Move(float speed) =>
                pers.transform.position = Vector3.MoveTowards(pers.transform.position, target, speed);
        }

        private void Start()
        {
            _time = _sec;
            StartCoroutine(timer());
        }

        private void OnValidate()
        {
            start.Clear();
            
            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                    start.Add(point.position + new Vector3(i * offset.x,offset.y,j* offset.z));
        }

        private void OnDrawGizmos()
        {
            foreach (var st in start)
            {
                Gizmos.DrawSphere(st, 0.1f);
            }
        }

        public void Ranger(Transform pers)
        {
            pers.transform.position = start[pres.Count];
            pers.transform.rotation = Quaternion.Euler(rot);
        }

        private void Update()
        {
            if (_timeStop > 0)
            {
                _timeStop -= Time.deltaTime;
                return;
            }

            if (_time > 0)
            {
                _time -= Time.deltaTime;
            }
            else
            {
                _time = _sec;

                foreach (var item in pres)
                    item.pers.Smoke();

                _timeStop = 11f / 4;
            }
        }

        private IEnumerator timer()
        {
            var s = new WaitForSeconds(1);
            while (true)
            {
                yield return s;
                var v = 0;
                foreach (var item in pres)
                    v += item.add;

                OnAdd?.Invoke(v);
            }
        }
    }
}