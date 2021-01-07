using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {}

[System.Serializable]
public struct Stats
{
    public int hp;
    public int xp;
    public float hunger;
    public float stamina;
}

[System.Serializable]
public class GameData
{
    public string name;
    public string title;

    [Multiline]
    public string description;

    public Sprite image;

    public bool isCharacterModel;
    public bool isFriendly;
    public bool isEnemy;

    public GameObject modelData;

    public Stats stats;

    public int currentInventorySize;
    public int nextInventorySize;
    public int maxInventorySize;
    public float inventoryUpgradeCost;

    public List<Inventory> startingInventory;
}

[CreateAssetMenu(fileName = "New Game Data", menuName = "Scriptables/Game Data Object")]
public class GameDataObject : ScriptableObject
{
    public List<GameData> gameData;
}
