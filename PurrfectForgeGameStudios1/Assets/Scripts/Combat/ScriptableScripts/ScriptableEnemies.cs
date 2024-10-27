using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu]

public class ScriptableEnemies : ScriptableObject
{
    // Start is called before the first frame update
    public enum Type { Normal, Ranged, Boss, Wave}
    public Type type;
    public string EnemyName;
    public int Level;
    public float HP;
    public float Defense;
    public float LineOfSight;
    public float MeleeAttackDistance;
    public float XpDrop;
    public float BossAttackPause;


    public ScriptableItems[] lootPool;



    public GameObject objectToShoot;
    public float bulletSpeed;
    public float DestroyTime;
    public float ShootRate;
}

