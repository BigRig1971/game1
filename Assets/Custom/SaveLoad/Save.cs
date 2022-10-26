using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<Vector3Serializable> targetPositions = new List<Vector3Serializable>();
    public List<QuaternionSerializable> targetRotation = new List<QuaternionSerializable>();
    public List<Strings> _strings = new List<Strings>();
    public List<InventoryItems> _inventoryItems = new List<InventoryItems>();
    
}
[System.Serializable]
public struct Vector3Serializable
{
    public float x;
    public float y;
    public float z;
}
[System.Serializable]
public struct QuaternionSerializable
{
    public float x;
    public float y;
    public float z;
    public float w;
}
[System.Serializable]
public struct Strings
{
    public string _string;
   
}
[System.Serializable]
public struct InventoryItems
{
    public string _item;

}


