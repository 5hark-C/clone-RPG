using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int currency;

    public SerializeableDictionary<string, bool> skillTree;
    public SerializeableDictionary<string, int> inventory;
    public List<string> equipmentID;

    public SerializeableDictionary<string, bool> checkpoints;
    public string closestCheckpointID;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializeableDictionary<string, float> volumeSettings;

    public GameData()
    {
        lostCurrencyX = 0;
        lostCurrencyY = 0;
        lostCurrencyAmount = 0;

        this.currency = 0;
        skillTree = new SerializeableDictionary<string, bool>();
        inventory = new SerializeableDictionary<string, int>();
        equipmentID = new List<string>();

        closestCheckpointID = string.Empty;
        checkpoints = new SerializeableDictionary<string, bool>();
        volumeSettings = new SerializeableDictionary<string, float>();
    }
}
