using System.Collections;
using Code.Grid;
using Code.Parametars;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TMP_Text heppyTx;
        [SerializeField] private Slider heppySl;
        [SerializeField] private TMP_Text coinTx;
        [SerializeField] private TMP_Text indastrTx;
        [SerializeField] private TMP_Text ecoTx;
        [SerializeField] private Button btRestart;
        [SerializeField] private Button btNext;
        [SerializeField] private GameObject panel;

        [SerializeField] private Happy happy;
        [SerializeField] private Coins coins;
        [SerializeField] private Indastriality indastrial;
        [SerializeField] private Ecologi eco;

        private void Start()
        {
            btRestart.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
            btNext.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });

            spawner.OnLoadStep += step =>
            {
                if (step != 7) return;

                panel.SetActive(true);
                View();
                StartCoroutine(AnimV());
            };
        }

        private IEnumerator AnimV()
        {
            var v = happy._value;
            var s = new WaitForSeconds(0.01f);
            while (v > 0)
            {
                yield return s;
                v--;

                coins._value++;
                indastrial._value++;
                eco._value++;

                View();
            }

            heppyTx.gameObject.SetActive(false);
            heppySl.gameObject.SetActive(false);

            btRestart.gameObject.SetActive(true);
            btNext.gameObject.SetActive(true);
        }

        private void View()
        {
            heppySl.value = happy._value / happy.valueInit;
            heppyTx.text = happy._value.ToString();
            coinTx.text = coins._value.ToString();
            indastrTx.text = indastrial._value.ToString();
            ecoTx.text = eco._value.ToString();
        }
    }
}