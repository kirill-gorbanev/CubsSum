using System;
using System.Collections.Generic;
using Code.Grid.Form;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class TypeCell : SerializedScriptableObject
{
    public TypeRes res;
    public List<Compatible> compatible;
    public Vector2 range;
    public Color view;
    public string info;
}

[Serializable]
public struct Compatible
{
    public TypeCell id;
  [SerializeReference]  public IConnectCells connect;
}

public interface IConnectCells
{
    
}


public struct Sum : IConnectCells{}