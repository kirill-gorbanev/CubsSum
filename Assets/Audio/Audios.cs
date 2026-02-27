using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class Audios : MonoBehaviour
    {
        [SerializeField] private AudioClip red;
        [SerializeField] private AudioClip green;
        [SerializeField] private ZoneController zone;

        [SerializeField] private AudioClip bye;
        [SerializeField] private AudioSource allSo;

        [SerializeField] private Toggle soundToggle;
        [SerializeField] private GameObject a;
        [SerializeField] private GameObject b;


        private List<AudioSource> _pool = new();


        private void Start()
        {
            zone.OnChange += e =>
            {
                var a = GetFromPool();
                a.clip = e ? green : red;
                a.Play();
                StartCoroutine(ReturnToPoolAfterPlay(a));
            };

            soundToggle.onValueChanged.AddListener(e =>
            {
                AudioListener.volume = e ? 1f : 0f;
                a.SetActive(e);
                b.SetActive(!e);
            });
        }

        public void Bye()
        {
            var a = GetFromPool();

            a.clip = bye;
            a.Play();
            StartCoroutine(ReturnToPoolAfterPlay(a));
        }

        private AudioSource GetFromPool()
        {
            AudioSource source = null;

            for (int i = 0; i < _pool.Count; i++)
            {
                if (!_pool[i].isPlaying && !_pool[i].gameObject.activeSelf)
                {
                    source = _pool[i];
                    break;
                }
            }

            if (source == null)
            {
                source = CreateNewSource();
            }

            source.gameObject.SetActive(true);
            return source;
        }

        private AudioSource CreateNewSource()
        {
            var newSource = Instantiate(allSo);
            newSource.gameObject.SetActive(false);
            _pool.Add(newSource);
            return newSource;
        }

        private IEnumerator ReturnToPoolAfterPlay(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);

            if (source.isPlaying)
            {
                yield return null;
            }

            source.gameObject.SetActive(false);
        }
    }
}