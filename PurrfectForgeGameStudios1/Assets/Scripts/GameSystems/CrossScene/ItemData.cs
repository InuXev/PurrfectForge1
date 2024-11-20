using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ItemData : MonoBehaviour
{
    [Header("Basic Information")]
    public string itemName;
    public string type;
    public string description;
    public int amountHeld;

    [Header("Eterius Building Information")]
    public ScriptableItems scriptableItems;
    public string eshesBuildObjectName;

    [Header("Weapon Stat Information")]
    public float HpPow;
    public float AttPow;
    public float DefPow;
    public float DexPow;
    public float StaminaPow;
    public int value;
}
