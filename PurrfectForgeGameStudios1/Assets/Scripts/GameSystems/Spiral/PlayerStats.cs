using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public float HP;
    public float HPOriginal;
    public float moveSpeed;
    private float moveSpeedOriginal;
    public float dashMult;
}
