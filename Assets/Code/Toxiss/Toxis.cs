using System;
using UnityEngine;

public class Toxis : MonoBehaviour
{
    [SerializeField] public double maxTox = 1;

    public int mult = 1;
    public int toxis;

    public event Action OnTox;

    public float V => toxis / (float)maxTox;

    public void Add()
    {
        toxis += mult;
        if (toxis >= maxTox)
            OnTox?.Invoke();

    }
}