using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Pers
{
    public class Moves : MonoBehaviour
    {
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private float speed;

        public List<MyStruct> pres = new();

        private float _time;
        private float _sec = 1f;

        public event Action<int> OnAdd; 
        
        public class MyStruct
        {
            public Transform pers;
            public Vector3 target;
            public int add;

            public bool IsNext => Vector3.Distance(pers.position, target) < 1f;
            public void Move(float speed) => pers.position = Vector3.MoveTowards(pers.position, target, speed);
        }

        private void Start()
        {
            _time = _sec;
        }

        public void Ranger()
        {
            foreach (var item in pres)
            {
                item.target = Range();
                item.pers.LookAt(item.target);
            }
        }
        
        private void Update()
        {
            foreach (var item in pres)
            {
                item.Move(speed * Time.deltaTime);
                if (item.IsNext)
                {
                    item.target = Range();
                    item.pers.LookAt(item.target);
                }
            }

            if (_time > 0)
            {
                _time -= Time.deltaTime;
            }
            else
            {
                _time = _sec;
                var v = 0;
                foreach (var item in pres)
                    v += item.add;
                OnAdd?.Invoke(v);
            }
        }

        public Vector3 Range()
        {
            var x = Random.Range(start.position.x, end.position.x);
            var y = Random.Range(start.position.y, end.position.y);
            var z = Random.Range(start.position.z, end.position.z);
            return new Vector3(x, y, z);
        }
    }
}