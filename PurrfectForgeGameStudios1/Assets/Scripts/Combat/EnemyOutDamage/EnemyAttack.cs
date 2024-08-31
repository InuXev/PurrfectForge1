using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float Damage;
    public static EnemyAttack Instance;
    public bool weaponUsed;

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Player") && !weaponUsed) // Check if the collided object is the player
            {
                PDamage dmg = other.gameObject.GetComponent<PDamage>();
                dmg.takeDamage(Damage); // Call the takeDamage function from the IDamage component
            }
    }
}
