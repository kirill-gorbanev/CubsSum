using UnityEngine;
using YG;
using YG.Utils.LB;

namespace Code
{
    public class LeaderLoad : MonoBehaviour
    {
        [SerializeField] private LeaderItem item;
        [SerializeField] private Transform parent;

        private void Awake()
        {
            YandexGame.onGetLeaderboard += e =>
            {
                for (int i = 0; i < parent.childCount; i++)
                    Destroy(parent.GetChild(i).gameObject);

                foreach (LBPlayerData lb in e.players)
                {
                    var i = Instantiate(item, parent);
                    i.Load(lb.name, lb.score, lb.photo);
                }
            };
        }
    }
}