using UnityEngine;

[CreateAssetMenu]
public class TypeRes : ScriptableObject
{
    public Vector2 rangeValue;
    
    public float Range => UnityEngine.Random.Range(rangeValue.x, rangeValue.y);
}