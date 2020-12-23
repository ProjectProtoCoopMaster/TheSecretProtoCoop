using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptables/Item Object")]
public class ItemObject : ScriptableObject
{
    public string itemName;

    public Sprite icon;

    public int cost;
    public float rarity;
    public float value;
    public float weight;

    public bool isUnique;
}
