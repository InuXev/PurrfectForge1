using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float Damage;
    public static EnemyAttack Instance;
    public bool weaponUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !weaponUsed) // Check if the collided object is the player and weapon hasn't been used
        {
            PDamage dmg = other.gameObject.GetComponent<PDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(Damage); // Apply damage to the player
                weaponUsed = true;
            }
        }
    }
}
