using System;
using System.Collections;
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
        private float _timeStop;
      [SerializeField]  private float _sec = 1f;

        public event Action<int> OnAdd;

        public class MyStruct
        {
            public Pers pers;
            public Vector3 target;
            public int add;

            public bool IsNext => Vector3.Distance(pers.transform.position, target) < 1f;

            public void Move(float speed) => pers.transform.position = Vector3.MoveTowards(pers.transform.position, target, speed);
        }

        private void Start()
        {
            _time = _sec;
            StartCoroutine( timer());
        }

        public void Ranger()
        {
            foreach (var item in pres)
            {
                item.pers.Move();
                item.target = Range();
                item.pers.transform.LookAt(item.target);
            }
        }

        private void Update()
        {
            if (_timeStop > 0)
            {
                _timeStop -= Time.deltaTime;
                return;
            }
            
            foreach (var item in pres)
            {
                item.Move(speed * Time.deltaTime);
                if (item.IsNext)
                {
                    item.target = Range();
                    item.pers.transform.LookAt(item.target);
                }
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

        public Vector3 Range()
        {
            var x = Random.Range(start.position.x, end.position.x);
            var y = Random.Range(start.position.y, end.position.y);
            var z = Random.Range(start.position.z, end.position.z);
            return new Vector3(x, y, z);
        }
    }
}