using System;
using System.Collections.Generic;
using Code.Grid.Form;
using Code.Grid.Form.Cell;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class TypeCell : SerializedScriptableObject
{
    public TypeRes res;
    public List<Compatible> compatible;
    public Vector2 range;
    public Color view;
    public string info;

    public Pointers pointers;


    public float Range => Random.Range(range.x, range.y);
}

[Serializable]
public struct Compatible
{
    public TypeCell id;
    [SerializeReference] public IConnectCells connect;
}

public interface IConnectCells
{
    public float GetValue(float baseValue);
}


public struct Mult : IConnectCells
{
    public Vector2 rangeSynMod;

    public float GetValue(float baseValue) => baseValue * UnityEngine.Random.Range(rangeSynMod.x, rangeSynMod.y);
}

public struct Sum : IConnectCells
{
    public Vector2 rangeSynMod;

    public float GetValue(float baseValue) => baseValue + UnityEngine.Random.Range(rangeSynMod.x, rangeSynMod.y);
}

public struct Min : IConnectCells
{
    public Vector2 rangeSynMod;

    public float GetValue(float baseValue) => baseValue - UnityEngine.Random.Range(rangeSynMod.x, rangeSynMod.y);
}

public struct PumpMod : IConnectCells
{
    public Vector2 rangeSynMod;
    public Vector2 pumpEnergy;

    public float GetValue(float baseValue) => baseValue + (UnityEngine.Random.Range(rangeSynMod.x, rangeSynMod.y) *
                                                           UnityEngine.Random.Range(pumpEnergy.x, pumpEnergy.y));
}