using UnityEngine;

public class Toxis : MonoBehaviour
{
    [SerializeField] private ZoneController zone;
    [SerializeField] public int maxTox = 1;

    public int toxis;

    private void Awake()
    {
        zone.OnChange += e => toxis += (e ? -1 : 1);
    }
}