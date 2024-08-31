using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedDamage : MonoBehaviour
{
    [SerializeField] float Damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
        {
            MDamage dmg = collision.gameObject.GetComponent<MDamage>();
            if (dmg != null)
            {
                dmg.takeMDamage(Damage); // Call the takeDamage function from the PDamage component
            }
        }
        Destroy(gameObject);
    }
}
