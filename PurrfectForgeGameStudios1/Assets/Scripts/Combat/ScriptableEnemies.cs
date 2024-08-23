using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu]

public class ScriptableEnemies : ScriptableObject
{
    // Start is called before the first frame update
    public enum Type { Normal, Floating, Boss}
    public Type type;
    public string EnemyName;
    public float Level;
    public float HP;
    public float Defense;
    public float LineOfSight;
    public float MeleeAttackDistance;
    public float XpDrop;
    public ScriptableItems[] lootPool;
}

