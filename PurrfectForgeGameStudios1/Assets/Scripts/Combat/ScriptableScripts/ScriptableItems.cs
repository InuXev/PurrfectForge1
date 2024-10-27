using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu]

public class ScriptableItems : ScriptableObject
{
    // Start is called before the first frame update
    public string type;
    public string itemName;
    public Sprite icon;
    public string description;
    public GameObject eshesBuildObject;
    public GameObject loot;
    public int amountHeld;
    public float dropChance;
    public ItemData itemData;
}
