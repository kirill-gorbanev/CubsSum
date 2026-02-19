using System;
using Code;
using UnityEngine;

public class Toxis : MonoBehaviour
{
    [SerializeField] private ZoneController zone;

    [SerializeField] public int maxTox = 1;

    public int mult = 1;
    public int toxis;

    public event Action OnTox;

    public void Add()
    {
        toxis += mult;
        if (toxis >= maxTox)
            OnTox?.Invoke();
    }
}